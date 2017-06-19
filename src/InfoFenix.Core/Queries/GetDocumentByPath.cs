using System.Threading;
using System.Threading.Tasks;
using InfoFenix.Core.Cqrs;
using InfoFenix.Core.Data;
using InfoFenix.Core.Dto;
using InfoFenix.Core.Entities;
using SQL = InfoFenix.Core.Resources.Resources;

namespace InfoFenix.Core.Queries {

    public sealed class GetDocumentByPathQuery : IQuery<DocumentDto> {

        #region Public Properties

        public string Path { get; set; }

        #endregion Public Properties
    }

    public sealed class GetDocumentByPathQueryHandler : IQueryHandler<GetDocumentByPathQuery, DocumentDto> {

        #region Private Read-Only Fields

        private readonly IDatabase _database;

        #endregion Private Read-Only Fields

        #region Public Constructors

        public GetDocumentByPathQueryHandler(IDatabase database) {
            Prevent.ParameterNull(database, nameof(database));

            _database = database;
        }

        #endregion Public Constructors

        #region IQueryHandler<GetDocumentByPathQuery, DocumentDto> Members

        public Task<DocumentDto> HandleAsync(GetDocumentByPathQuery query, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => {
                return _database.ExecuteReaderSingle(SQL.GetDocumentByPath, DocumentDto.Map, parameters: new[] {
                    Parameter.CreateInputParameter(nameof(DocumentEntity.Path), query.Path)
                });
            }, cancellationToken);
        }

        #endregion IQueryHandler<GetDocumentByPathQuery, DocumentDto> Members
    }
}