using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InfoFenix.Core.Cqrs;
using InfoFenix.Core.Data;
using InfoFenix.Core.Dto;
using Resource = InfoFenix.Core.Resources.Resources;

namespace InfoFenix.Core.Queries {

    public sealed class GetDocumentDirectoryByPathQuery : IQuery<DocumentDirectoryDto> {

        #region Public Properties

        public string Path { get; set; }

        public bool RequireDocuments { get; set; }

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
                var documentDirectory = _database.ExecuteReaderSingle(Resource.GetDocumentDirectoryByPathSQL, DocumentDirectoryDto.Map, parameters: new[] {
                    Parameter.CreateInputParameter(Common.DatabaseSchema.DocumentDirectories.Path, query.Path)
                });

                if (query.RequireDocuments) {
                    documentDirectory.Documents = _database.ExecuteReader(Resource.ListDocumentsByDocumentDirectorySQL, (reader) => DocumentDto.Map(reader, documentDirectory), parameters: new[] {
                        Parameter.CreateInputParameter(Common.DatabaseSchema.Documents.DocumentDirectoryID, documentDirectory.DocumentDirectoryID, DbType.Int32)
                    }).ToList();
                }

                return documentDirectory;
            }, cancellationToken);
        }

        #endregion IQueryHandler<GetDocumentDirectoryByPathQuery, DocumentDirectoryDto> Members
    }
}