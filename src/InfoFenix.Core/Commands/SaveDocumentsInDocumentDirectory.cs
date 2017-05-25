using System;
using System.Collections.Generic;
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

        public string DocumentDirectoryLabel { get; set; }

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

        private void SaveDocumentEntries(int documentDirectoryID, string documentDirectoryPath, IEnumerable<DocumentEntity> documents, IEnumerable<string> physicalFiles) {
            var physicalFilesCount = physicalFiles.Count();
            _pubSub.PublishAsync(new DocumentsInDirectoryToSaveNotification {
                DocumentDirectoryLabel = string.Empty,
                TotalDocuments = physicalFilesCount
            });
            _pubSub.PublishAsync(new StartingProgressiveTaskNotification {
                Title = "Salvar Documentos",
                Message = $"Salvando documentos do diretório: {documentDirectoryPath}",
                MinimunValue = 1,
                MaximunValue = physicalFilesCount
            });
            var counter = 1;
            foreach (var file in physicalFiles) {
                var document = documents.SingleOrDefault(_ => string.Equals(_.Path, file, StringComparison.InvariantCultureIgnoreCase));
                _database.ExecuteScalar(SQL.SaveDocument, parameters: new[] {
                    Parameter.CreateInputParameter(nameof(DocumentEntity.ID), document != null ? (object)document.ID : DBNull.Value, DbType.Int32),
                    Parameter.CreateInputParameter(nameof(DocumentEntity.DocumentDirectoryID), documentDirectoryID, DbType.Int32),
                    Parameter.CreateInputParameter(nameof(DocumentEntity.Path), file),
                    Parameter.CreateInputParameter(nameof(DocumentEntity.LastWriteTime), File.GetLastWriteTime(file), DbType.DateTime),
                    Parameter.CreateInputParameter(nameof(DocumentEntity.Code), Common.ExtractCodeFromFilePath(file), DbType.Int32),
                    Parameter.CreateInputParameter(nameof(DocumentEntity.Indexed), 0, DbType.Int32),
                    Parameter.CreateInputParameter(nameof(DocumentEntity.Payload), File.ReadAllBytes(file), DbType.Binary)
                });
                _pubSub.PublishAsync(new DocumentSavedNotification {
                    FullPath = file
                });
                _pubSub.PublishAsync(new ProgressiveTaskPerformStepNotification {
                    Title = "Salvar Documentos",
                    Message = $"Documento salvo: {Path.GetFileNameWithoutExtension(file)}",
                    ActualStep = counter,
                    StepsToCompletation = physicalFilesCount
                });
                counter++;
            }
        }
        
        private void RemoveDocumentEntries(SaveDocumentsInDocumentDirectoryCommand command, IEnumerable<DocumentEntity> documents, IEnumerable<string> toRemove) {
            var toRemoveCount = toRemove.Count();
            _pubSub.PublishAsync(new DocumentsInDirectoryToRemoveNotification {
                DocumentDirectoryLabel = string.Empty,
                TotalDocuments = toRemoveCount
            });
            if (toRemoveCount > 0) {
                _pubSub.PublishAsync(new StartingProgressiveTaskNotification {
                    Title = "Remover Documentos",
                    Message = $"Removendo documentos do diretório: {command.DocumentDirectoryPath}",
                    MinimunValue = 1,
                    MaximunValue = toRemoveCount
                });
                var index = _indexProvider.GetOrCreate(command.DocumentDirectoryCode);
                var counter = 1;
                foreach (var file in toRemove) {
                    var document = documents.Single(_ => string.Equals(_.Path, file, StringComparison.InvariantCultureIgnoreCase));
                    _database.ExecuteScalar(SQL.RemoveDocument, parameters: new[] {
                        Parameter.CreateInputParameter(nameof(DocumentEntity.ID), document.ID, DbType.Int32)
                    });
                    index.DeleteDocuments(document.ID.ToString());
                    _pubSub.PublishAsync(new DocumentRemovedNotification {
                        FullPath = file
                    });
                    _pubSub.PublishAsync(new ProgressiveTaskPerformStepNotification {
                        Title = "Remover Documentos",
                        Message = $"Documento removido: {Path.GetFileNameWithoutExtension(file)}",
                        ActualStep = counter,
                        StepsToCompletation = toRemoveCount
                    });
                    counter++;
                }
            }
        }

        #endregion Private Methods

        #region ICommandHandler<SaveDocumentsInDocumentDirectoryCommand> Members

        public void Handle(SaveDocumentsInDocumentDirectoryCommand command) {
            var physicalFiles = Common.GetDocFiles(command.DocumentDirectoryPath);
            var documents = _database.ExecuteReader(SQL.ListDocumentsByDocumentDirectory, DocumentEntity.MapFromDataReader, parameters: new[] {
                Parameter.CreateInputParameter(nameof(DocumentDirectoryEntity.ID), command.DocumentDirectoryID, DbType.Int32)
            }).ToArray();
            using (var transaction = _database.Connection.BeginTransaction()) {
                try {
                    SaveDocumentEntries(command.DocumentDirectoryID, command.DocumentDirectoryPath, documents, physicalFiles);

                    var databaseFiles = documents.Select(_ => _.Path).ToArray();
                    var toRemove = databaseFiles.Except(physicalFiles);
                    RemoveDocumentEntries(command, documents, toRemove);

                    transaction.Commit();
                } catch (Exception ex) { Log.Error(ex, ex.Message); transaction.Rollback(); throw; }
            }
        }

        #endregion ICommandHandler<SaveDocumentsInDocumentDirectoryCommand> Members
    }
}