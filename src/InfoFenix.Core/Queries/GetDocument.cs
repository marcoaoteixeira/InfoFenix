using System.Data;
using InfoFenix.Core.Cqrs;
using InfoFenix.Core.Data;
using InfoFenix.Core.Entities;
using SQL = InfoFenix.Core.Resources.Resources;

namespace InfoFenix.Core.Queries {

    public sealed class GetDocumentQuery : IQuery<DocumentEntity> {

        #region Public Properties

        public int ID { get; set; }

        #endregion Public Properties
    }

    public sealed class GetDocumentQueryHandler : IQueryHandler<GetDocumentQuery, DocumentEntity> {

        #region Private Read-Only Fields

        private readonly IDatabase _database;

        #endregion Private Read-Only Fields

        #region Public Constructors

        public GetDocumentQueryHandler(IDatabase database) {
            Prevent.ParameterNull(database, nameof(database));

            _database = database;
        }

        #endregion Public Constructors

        #region IQueryHandler<GetDocumentQuery, DocumentEntity> Members

        public DocumentEntity Handle(GetDocumentQuery query) {
            return _database.ExecuteReaderSingle(SQL.GetDocument, DocumentEntity.MapFromDataReader, parameters: new[] {
                Parameter.CreateInputParameter(nameof(DocumentEntity.ID), query.ID, DbType.Int32)
            });
        }

        #endregion IQueryHandler<GetDocumentQuery, DocumentEntity> Members
    }
}