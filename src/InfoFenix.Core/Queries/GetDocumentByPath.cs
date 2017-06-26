using System.Data;
using System.Threading;
using System.Threading.Tasks;
using InfoFenix.Core.Cqrs;
using InfoFenix.Core.Data;
using InfoFenix.Core.Dto;
using Resource = InfoFenix.Core.Resources.Resources;

namespace InfoFenix.Core.Queries {

    public sealed class GetDocumentByPathQuery : IQuery<DocumentDto> {

        #region Public Properties

        public string Path { get; set; }

        public bool RequireDocumentDirectory { get; set; }

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
                var document = _database.ExecuteReaderSingle(Resource.GetDocumentByPathSQL, DocumentDto.Map, parameters: new[] {
                    Parameter.CreateInputParameter(Common.DatabaseSchema.Documents.Path, query.Path)
                });

                var documentDirectory = query.RequireDocumentDirectory
                    ? _database.ExecuteReaderSingle(Resource.GetDocumentDirectorySQL, (reader) => DocumentDirectoryDto.Map(reader, new[] { document }), parameters: new[] {
                        Parameter.CreateInputParameter(Common.DatabaseSchema.DocumentDirectories.DocumentDirectoryID, document.DocumentDirectory.DocumentDirectoryID, DbType.Int32)
                    })
                    : null;

                document.DocumentDirectory = documentDirectory;

                return document;
            }, cancellationToken);
        }

        #endregion IQueryHandler<GetDocumentByPathQuery, DocumentDto> Members
    }
}