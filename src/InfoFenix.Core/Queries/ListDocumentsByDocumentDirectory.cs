using System;
using System.Collections.Generic;
using System.Data;
using InfoFenix.Core;
using InfoFenix.Core.Cqrs;
using InfoFenix.Core.Data;
using InfoFenix.Core.Entities;
using SQL = InfoFenix.Core.Resources.Resources;

namespace InfoFenix.Core.Queries {
    public class ListDocumentsByDocumentDirectoryQuery : IQuery<IEnumerable<DocumentEntity>> {

        #region Public Properties

        public int DocumentDirectoryID { get; set; }

        #endregion Public Properties
    }

    public class ListDocumentsByDocumentDirectoryQueryHandler : IQueryHandler<ListDocumentsByDocumentDirectoryQuery, IEnumerable<DocumentEntity>> {

        #region Private Read-Only Fields

        private readonly IDatabase _database;

        #endregion Private Read-Only Fields

        #region Public Constructors

        public ListDocumentsByDocumentDirectoryQueryHandler(IDatabase database) {
            Prevent.ParameterNull(database, nameof(database));

            _database = database;
        }

        #endregion Public Constructors

        #region Private Static Methods

        private static DocumentEntity Map(IDataReader reader) {
            return new DocumentEntity {
                DocumentID = reader.GetInt32OrDefault("document_id"),
                DocumentDirectoryID = reader.GetInt32OrDefault("document_directory_id"),
                FullPath = reader.GetStringOrDefault("full_path"),
                Code = reader.GetInt32OrDefault("code"),
                LastWriteTime = reader.GetDateTimeOrDefault("last_write_time", DateTime.MinValue),
                Indexed = reader.GetInt32OrDefault("indexed") > 0,
                Payload = reader.GetBlobOrDefault("payload")
            };
        }

        #endregion Private Static Methods

        #region IQueryHandler<ListDocumentsByDocumentDirectoryQuery, IEnumerable<DocumentEntity>> Members

        public IEnumerable<DocumentEntity> Handle(ListDocumentsByDocumentDirectoryQuery query) {
            return _database.ExecuteReader(SQL.ListDocumentsByDocumentDirectory, Map, parameters: new[] {
                Parameter.CreateInputParameter(nameof(DocumentDirectoryEntity.DocumentDirectoryID), query.DocumentDirectoryID, DbType.Int32)
            });
        }

        #endregion IQueryHandler<ListDocumentsByDocumentDirectoryQuery, IEnumerable<DocumentEntity>> Members
    }
}