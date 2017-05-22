using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using InfoFenix.Core.Cqrs;
using InfoFenix.Core.Data;
using InfoFenix.Core.Entities;
using InfoFenix.Core.Search;
using SQL = InfoFenix.Core.Resources.Resources;

namespace InfoFenix.Core.Commands {

    public class SaveDocumentDirectoryDocumentsCommand : ICommand {

        #region Public Properties

        public int DocumentDirectoryID { get; set; }

        public string DocumentDirectoryDirectoryPath { get; set; }

        public string DocumentDirectoryCode { get; set; }

        #endregion Public Properties
    }

    public class SaveDocumentDirectoryDocumentsCommandHandler : ICommandHandler<SaveDocumentDirectoryDocumentsCommand> {

        #region Private Read-Only Fields

        private readonly IDatabase _database;
        private readonly IIndexProvider _indexProvider;

        #endregion Private Read-Only Fields

        #region Public Constructors

        public SaveDocumentDirectoryDocumentsCommandHandler(IDatabase database, IIndexProvider indexProvider) {
            Prevent.ParameterNull(database, nameof(database));
            Prevent.ParameterNull(indexProvider, nameof(indexProvider));

            _database = database;
            _indexProvider = indexProvider;
        }

        #endregion Public Constructors

        #region ICommandHandler<SaveDocumentDirectoryDocumentsCommand> Members

        public void Handle(SaveDocumentDirectoryDocumentsCommand command) {
            var physicalFiles = Common.GetDocFiles(command.DocumentDirectoryDirectoryPath);
            var documents = _database.ExecuteReader(SQL.ListDocumentsByDocumentDirectory, DocumentEntity.MapFromDataReader, parameters: new[] {
                Parameter.CreateInputParameter(nameof(DocumentDirectoryEntity.DocumentDirectoryID), command.DocumentDirectoryID, DbType.Int32)
            });
            var databaseFiles = documents.Select(_ => _.FullPath).ToArray();

            var toCreate = physicalFiles.Except(databaseFiles);
            foreach (var file in toCreate) {
                var codeCapture = Regex.Match(Path.GetFileNameWithoutExtension(file), "([0-9]{1,})");
                _database.ExecuteScalar(SQL.SaveDocument, parameters: new[] {
                    Parameter.CreateInputParameter(nameof(DocumentEntity.DocumentID), DBNull.Value, DbType.Int32),
                    Parameter.CreateInputParameter(nameof(DocumentEntity.DocumentDirectoryID), command.DocumentDirectoryID, DbType.Int32),
                    Parameter.CreateInputParameter(nameof(DocumentEntity.FullPath), file),
                    Parameter.CreateInputParameter(nameof(DocumentEntity.LastWriteTime), File.GetLastWriteTime(file), DbType.DateTime),
                    Parameter.CreateInputParameter(nameof(DocumentEntity.Code), codeCapture.Captures.Count > 0 ? int.Parse(codeCapture.Captures[0].Value) : 0, DbType.Int32),
                    Parameter.CreateInputParameter(nameof(DocumentEntity.Indexed), 0, DbType.Int32),
                    Parameter.CreateInputParameter(nameof(DocumentEntity.Payload), File.ReadAllBytes(file), DbType.Binary)
                });
            }

            var toRemove = databaseFiles.Except(physicalFiles);
            var index = _indexProvider.GetOrCreate(command.DocumentDirectoryCode);
            foreach (var file in toRemove) {
                var document = documents.Single(_ => _.FullPath == file);
                _database.ExecuteScalar(SQL.RemoveDocument, parameters: new[] {
                    Parameter.CreateInputParameter(nameof(DocumentEntity.DocumentID), document.DocumentID, DbType.Int32)
                });
                index.DeleteDocuments(document.DocumentID.ToString());
            }
        }

        #endregion ICommandHandler<SaveDocumentDirectoryDocumentsCommand> Members
    }
}