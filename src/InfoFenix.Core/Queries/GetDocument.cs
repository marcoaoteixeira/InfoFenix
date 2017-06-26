using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using InfoFenix.Core.Cqrs;
using InfoFenix.Core.Data;
using InfoFenix.Core.Dto;
using Resource = InfoFenix.Core.Resources.Resources;

namespace InfoFenix.Core.Queries {

    public sealed class GetDocumentQuery : IQuery<DocumentDto> {

        #region Public Properties

        public int ID { get; set; }

        public int Code { get; set; }

        public bool RequireDocumentDirectory { get; set; }

        #endregion Public Properties
    }

    public sealed class GetDocumentQueryHandler : IQueryHandler<GetDocumentQuery, DocumentDto> {

        #region Private Read-Only Fields

        private readonly IDatabase _database;

        #endregion Private Read-Only Fields

        #region Public Constructors

        public GetDocumentQueryHandler(IDatabase database) {
            Prevent.ParameterNull(database, nameof(database));

            _database = database;
        }

        #endregion Public Constructors

        #region IQueryHandler<GetDocumentQuery, DocumentDto> Members

        public Task<DocumentDto> HandleAsync(GetDocumentQuery query, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => {
                var document = _database.ExecuteReaderSingle(Resource.GetDocumentSQL, DocumentDto.Map, parameters: new[] {
                    Parameter.CreateInputParameter(Common.DatabaseSchema.Documents.DocumentID, query.ID != 0 ? (object)query.ID : DBNull.Value, DbType.Int32),
                    Parameter.CreateInputParameter(Common.DatabaseSchema.Documents.Code, query.Code != 0 ? (object)query.Code : DBNull.Value, DbType.Int32)
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

        #endregion IQueryHandler<GetDocumentQuery, DocumentDto> Members
    }
}