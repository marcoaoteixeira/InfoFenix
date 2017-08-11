using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using InfoFenix.Core.CQRS;
using InfoFenix.Core.Data;
using InfoFenix.Core.Entities;
using Resource = InfoFenix.Core.Resources.Resources;

namespace InfoFenix.Core.Queries {

    public sealed class GetDocumentQuery : IQuery<Document> {

        #region Public Properties

        public int ID { get; set; }

        public int Code { get; set; }

        public bool FetchPayload { get; set; }

        #endregion Public Properties
    }

    public sealed class GetDocumentQueryHandler : IQueryHandler<GetDocumentQuery, Document> {

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

        public Task<Document> HandleAsync(GetDocumentQuery query, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => {
                return _database.ExecuteReaderSingle(query.FetchPayload ? Resource.GetDocumentSQL : Resource.GetDocumentSQLWithoutPayloadSQL, Document.Map, parameters: new[] {
                    Parameter.CreateInputParameter(Common.DatabaseSchema.Documents.DocumentID, query.ID != 0 ? (object)query.ID : DBNull.Value, DbType.Int32),
                    Parameter.CreateInputParameter(Common.DatabaseSchema.Documents.Code, query.Code != 0 ? (object)query.Code : DBNull.Value, DbType.Int32)
                });
            }, cancellationToken);
        }

        #endregion IQueryHandler<GetDocumentQuery, DocumentDto> Members
    }
}