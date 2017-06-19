using System.Data;
using System.Threading;
using System.Threading.Tasks;
using InfoFenix.Core.Cqrs;
using InfoFenix.Core.Data;
using InfoFenix.Core.Dto;
using Resource = InfoFenix.Core.Resources.Resources;

namespace InfoFenix.Core.Queries {

    public sealed class GetDocumentDirectoryQuery : IQuery<DocumentDirectoryDto> {

        #region Public Properties

        public int ID { get; set; }

        #endregion Public Properties
    }

    public sealed class GetDocumentDirectoryQueryHandler : IQueryHandler<GetDocumentDirectoryQuery, DocumentDirectoryDto> {

        #region Private Read-Only Fields

        private readonly IDatabase _database;

        #endregion Private Read-Only Fields

        #region Public Constructors

        public GetDocumentDirectoryQueryHandler(IDatabase database) {
            Prevent.ParameterNull(database, nameof(database));

            _database = database;
        }

        #endregion Public Constructors

        #region IQueryHandler<GetDocumentDirectoryQuery, DocumentDirectoryDto> Members

        public Task<DocumentDirectoryDto> HandleAsync(GetDocumentDirectoryQuery query, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => {
                return _database.ExecuteReaderSingle(Resource.GetDocumentDirectorySQL, DocumentDirectoryDto.Map, parameters: new[] {
                    Parameter.CreateInputParameter(Common.DatabaseSchema.DocumentDirectories.DocumentDirectoryID, query.ID, DbType.Int32)
                });
            }, cancellationToken);
        }

        #endregion IQueryHandler<GetDocumentDirectoryQuery, DocumentDirectoryDto> Members
    }
}