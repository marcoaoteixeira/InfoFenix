using System.Collections.Generic;
using System.Data;
using InfoFenix.Core;
using InfoFenix.Core.Cqrs;
using InfoFenix.Core.Data;
using InfoFenix.Core.Entities;
using SQL = InfoFenix.Core.Resources.Resources;

namespace InfoFenix.Core.Queries {
    public class ListDocumentDirectoriesQuery : IQuery<IEnumerable<DocumentDirectoryEntity>> {

        #region Public Properties

        public string Label { get; set; }

        public string DirectoryPath { get; set; }

        public string Code { get; set; }

        public bool Watch { get; set; }

        public bool Index { get; set; }

        #endregion Public Properties
    }

    public class ListDocumentDirectoriesQueryHandler : IQueryHandler<ListDocumentDirectoriesQuery, IEnumerable<DocumentDirectoryEntity>> {

        #region Private Read-Only Fields

        private readonly IDatabase _database;

        #endregion Private Read-Only Fields

        #region Public Constructors

        public ListDocumentDirectoriesQueryHandler(IDatabase database) {
            Prevent.ParameterNull(database, nameof(database));

            _database = database;
        }

        #endregion Public Constructors

        #region Private Static Methods

        private static DocumentDirectoryEntity Map(IDataReader reader) {
            return new DocumentDirectoryEntity {
                DocumentDirectoryID = reader.GetInt32OrDefault("document_directory_id"),
                Label = reader.GetStringOrDefault("label"),
                DirectoryPath = reader.GetStringOrDefault("directory_path"),
                Code = reader.GetStringOrDefault("code"),
                Watch = reader.GetInt32OrDefault("watch") > 0,
                Index = reader.GetInt32OrDefault("index") > 0
            };
        }

        #endregion Private Static Methods

        #region IQueryHandler<ListDocumentDirectoriesQuery, IEnumerable<DocumentDirectoryEntity>> Members

        public IEnumerable<DocumentDirectoryEntity> Handle(ListDocumentDirectoriesQuery query) {
            return _database.ExecuteReader(SQL.ListDocumentDirectories, Map, parameters: new[] {
                Parameter.CreateInputParameter(nameof(DocumentDirectoryEntity.Label), query.Label),
                Parameter.CreateInputParameter(nameof(DocumentDirectoryEntity.DirectoryPath), query.DirectoryPath),
                Parameter.CreateInputParameter(nameof(DocumentDirectoryEntity.Code), query.Code),
                Parameter.CreateInputParameter(nameof(DocumentDirectoryEntity.Watch), query.Watch == true ? 1 : 0, DbType.Int32),
                Parameter.CreateInputParameter(nameof(DocumentDirectoryEntity.Index), query.Index == true ? 1 : 0, DbType.Int32)
            });
        }

        #endregion IQueryHandler<ListDocumentDirectoriesQuery, IEnumerable<DocumentDirectoryEntity>> Members
    }
}