using System.Data;
using InfoFenix.Core.Cqrs;
using InfoFenix.Core.Data;
using InfoFenix.Core.Entities;
using SQL = InfoFenix.Core.Resources.Resources;

namespace InfoFenix.Core.Queries {

    public sealed class GetDocumentDirectoryQuery : IQuery<DocumentDirectoryEntity> {

        #region Public Properties

        public int ID { get; set; }

        #endregion Public Properties
    }

    public sealed class GetDocumentDirectoryQueryHandler : IQueryHandler<GetDocumentDirectoryQuery, DocumentDirectoryEntity> {

        #region Private Read-Only Fields

        private readonly IDatabase _database;

        #endregion Private Read-Only Fields

        #region Public Constructors

        public GetDocumentDirectoryQueryHandler(IDatabase database) {
            Prevent.ParameterNull(database, nameof(database));

            _database = database;
        }

        #endregion Public Constructors

        #region IQueryHandler<GetDocumentDirectoryQuery, DocumentDirectoryEntity> Members

        public DocumentDirectoryEntity Handle(GetDocumentDirectoryQuery query) {
            return _database.ExecuteReaderSingle(SQL.GetDocumentDirectory, DocumentDirectoryEntity.MapFromDataReader, parameters: new[] {
                Parameter.CreateInputParameter(nameof(DocumentDirectoryEntity.ID), query.ID, DbType.Int32)
            });
        }

        #endregion IQueryHandler<GetDocumentDirectoryQuery, DocumentDirectoryEntity> Members
    }
}