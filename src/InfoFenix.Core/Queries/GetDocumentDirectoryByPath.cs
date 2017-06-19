using System.Threading;
using System.Threading.Tasks;
using InfoFenix.Core.Cqrs;
using InfoFenix.Core.Data;
using InfoFenix.Core.Dto;
using InfoFenix.Core.Entities;
using SQL = InfoFenix.Core.Resources.Resources;

namespace InfoFenix.Core.Queries {

    public sealed class GetDocumentDirectoryByPathQuery : IQuery<DocumentDirectoryDto> {

        #region Public Properties

        public string Path { get; set; }

        #endregion Public Properties
    }

    public sealed class GetDocumentDirectoryByPathQueryHandler : IQueryHandler<GetDocumentDirectoryByPathQuery, DocumentDirectoryDto> {

        #region Private Read-Only Fields

        private readonly IDatabase _database;

        #endregion Private Read-Only Fields

        #region Public Constructors

        public GetDocumentDirectoryByPathQueryHandler(IDatabase database) {
            Prevent.ParameterNull(database, nameof(database));

            _database = database;
        }

        #endregion Public Constructors

        #region IQueryHandler<GetDocumentDirectoryByPathQuery, DocumentDirectoryDto> Members

        public Task<DocumentDirectoryDto> HandleAsync(GetDocumentDirectoryByPathQuery query, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => {
                return _database.ExecuteReaderSingle(SQL.GetDocumentDirectoryByPath, DocumentDirectoryDto.Map, parameters: new[] {
                    Parameter.CreateInputParameter(nameof(DocumentDirectoryEntity.Path), query.Path)
                });
            }, cancellationToken);
        }

        #endregion IQueryHandler<GetDocumentDirectoryByPathQuery, DocumentDirectoryDto> Members
    }
}