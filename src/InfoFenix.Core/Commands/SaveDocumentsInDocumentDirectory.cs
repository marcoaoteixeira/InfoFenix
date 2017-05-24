using System;
using System.Data;
using System.IO;
using System.Linq;
using InfoFenix.Core.Cqrs;
using InfoFenix.Core.Data;
using InfoFenix.Core.Entities;
using InfoFenix.Core.Logging;
using InfoFenix.Core.PubSub;
using InfoFenix.Core.Search;
using SQL = InfoFenix.Core.Resources.Resources;

namespace InfoFenix.Core.Commands {

    public class SaveDocumentsInDocumentDirectoryCommand : ICommand {

        #region Public Properties

        public int DocumentDirectoryID { get; set; }

        public string DocumentDirectoryPath { get; set; }

        public string DocumentDirectoryCode { get; set; }

        #endregion Public Properties
    }

    public class SaveDocumentsInDocumentDirectoryCommandHandler : ICommandHandler<SaveDocumentsInDocumentDirectoryCommand> {

        #region Private Read-Only Fields

        private readonly IDatabase _database;
        private readonly IIndexProvider _indexProvider;
        private readonly IPublisherSubscriber _pubSub;

        #endregion Private Read-Only Fields

        #region Public Properties

        private ILogger _log;

        public ILogger Log {
            get { return _log ?? NullLogger.Instance; }
            set { _log = value ?? NullLogger.Instance; }
        }

        #endregion Public Properties

        #region Public Constructors

        public SaveDocumentsInDocumentDirectoryCommandHandler(IDatabase database, IIndexProvider indexProvider, IPublisherSubscriber pubSub) {
            Prevent.ParameterNull(database, nameof(database));
            Prevent.ParameterNull(indexProvider, nameof(indexProvider));
            Prevent.ParameterNull(pubSub, nameof(pubSub));

            _database = database;
            _indexProvider = indexProvider;
            _pubSub = pubSub;
        }

        #endregion Public Constructors

        #region Private Methods

        private void RemoveDocuments(SaveDocumentsInDocumentDirectoryCommand command, string[] physicalFiles, DocumentEntity[] documents, string[] databaseFiles) {
            var toRemove = databaseFiles.Except(physicalFiles).ToArray();
            _pubSub.PublishAsync(new DocumentsInDirectoryToRemoveNotification {
                DocumentDirectoryLabel = string.Empty,
                TotalDocuments = toRemove.Length
            });
            if (toRemove.Length > 0) {
                var index = _indexProvider.GetOrCreate(command.DocumentDirectoryCode);
                foreach (var file in toRemove) {
                    var document = documents.Single(_ => _.FullPath == file);
                    _database.ExecuteScalar(SQL.RemoveDocument, parameters: new[] {
                        Parameter.CreateInputParameter(nameof(DocumentEntity.DocumentID), document.DocumentID, DbType.Int32)
                    });
                    index.DeleteDocuments(document.DocumentID.ToString());
                    _pubSub.PublishAsync(new DocumentRemovedNotification {
                        FullPath = file
                    });
                }
            }
        }

        private void SaveDocuments(SaveDocumentsInDocumentDirectoryCommand command, string[] physicalFiles, string[] databaseFiles) {
            var toCreate = physicalFiles.Except(databaseFiles).ToArray();
            _pubSub.PublishAsync(new DocumentsInDirectoryToSaveNotification {
                DocumentDirectoryLabel = string.Empty,
                TotalDocuments = toCreate.Length
            });
            foreach (var file in toCreate) {
                _database.ExecuteScalar(SQL.SaveDocument, parameters: new[] {
                    Parameter.CreateInputParameter(nameof(DocumentEntity.DocumentID), DBNull.Value, DbType.Int32),
                    Parameter.CreateInputParameter(nameof(DocumentEntity.DocumentDirectoryID), command.DocumentDirectoryID, DbType.Int32),
                    Parameter.CreateInputParameter(nameof(DocumentEntity.FullPath), file),
                    Parameter.CreateInputParameter(nameof(DocumentEntity.LastWriteTime), File.GetLastWriteTime(file), DbType.DateTime),
                    Parameter.CreateInputParameter(nameof(DocumentEntity.Code), Common.ExtractCodeFromFilePath(file), DbType.Int32),
                    Parameter.CreateInputParameter(nameof(DocumentEntity.Indexed), 0, DbType.Int32),
                    Parameter.CreateInputParameter(nameof(DocumentEntity.Payload), File.ReadAllBytes(file), DbType.Binary)
                });
                _pubSub.PublishAsync(new DocumentSavedNotification {
                    FullPath = file
                });
            }
        }

        #endregion Private Methods

        #region ICommandHandler<SaveDocumentsInDocumentDirectoryCommand> Members

        public void Handle(SaveDocumentsInDocumentDirectoryCommand command) {
            var physicalFiles = Common.GetDocFiles(command.DocumentDirectoryPath);
            var documents = _database.ExecuteReader(SQL.ListDocumentsByDocumentDirectory, DocumentEntity.MapFromDataReader, parameters: new[] {
                Parameter.CreateInputParameter(nameof(DocumentDirectoryEntity.DocumentDirectoryID), command.DocumentDirectoryID, DbType.Int32)
            }).ToArray();
            var databaseFiles = documents.Select(_ => _.FullPath).ToArray();

            using (var transaction = _database.Connection.BeginTransaction(IsolationLevel.ReadUncommitted)) {
                try {
                    SaveDocuments(command, physicalFiles, databaseFiles);
                    RemoveDocuments(command, physicalFiles, documents, databaseFiles);
                    transaction.Commit();
                } catch (Exception ex) { Log.Error(ex, ex.Message); transaction.Rollback(); throw; }
            }
        }

        #endregion ICommandHandler<SaveDocumentsInDocumentDirectoryCommand> Members
    }
}