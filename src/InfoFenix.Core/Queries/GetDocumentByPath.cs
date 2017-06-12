using InfoFenix.Core.Cqrs;
using InfoFenix.Core.Data;
using InfoFenix.Core.Entities;
using SQL = InfoFenix.Core.Resources.Resources;

namespace InfoFenix.Core.Queries {

    public sealed class GetDocumentByPathQuery : IQuery<DocumentEntity> {

        #region Public Properties

        public string Path { get; set; }

        #endregion Public Properties
    }

    public sealed class GetDocumentByPathQueryHandler : IQueryHandler<GetDocumentByPathQuery, DocumentEntity> {

        #region Private Read-Only Fields

        private readonly IDatabase _database;

        #endregion Private Read-Only Fields

        #region Public Constructors

        public GetDocumentByPathQueryHandler(IDatabase database) {
            Prevent.ParameterNull(database, nameof(database));

            _database = database;
        }

        #endregion Public Constructors

        #region IQueryHandler<GetDocumentByPathQuery, DocumentEntity> Members

        public DocumentEntity Handle(GetDocumentByPathQuery query) {
            return _database.ExecuteReaderSingle(SQL.GetDocumentByPath, DocumentEntity.MapFromDataReader, parameters: new[] {
                Parameter.CreateInputParameter(nameof(DocumentEntity.Path), query.Path)
            });
        }

        #endregion IQueryHandler<GetDocumentByPathQuery, DocumentEntity> Members
    }
}