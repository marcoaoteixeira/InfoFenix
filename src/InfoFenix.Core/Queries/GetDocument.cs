using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using InfoFenix.Core.Cqrs;
using InfoFenix.Core.Data;
using InfoFenix.Core.Dto;
using InfoFenix.Core.Entities;
using SQL = InfoFenix.Core.Resources.Resources;

namespace InfoFenix.Core.Queries {

    public sealed class GetDocumentQuery : IQuery<DocumentDto> {

        #region Public Properties

        public int ID { get; set; }

        public int Code { get; set; }

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
                return _database.ExecuteReaderSingle(SQL.GetDocument, DocumentDto.Map, parameters: new[] {
                    Parameter.CreateInputParameter(nameof(DocumentEntity.ID), query.ID != 0 ? (object)query.ID : DBNull.Value, DbType.Int32),
                    Parameter.CreateInputParameter(nameof(DocumentEntity.Code), query.Code != 0 ? (object)query.Code : DBNull.Value, DbType.Int32)
                });
            }, cancellationToken);
        }

        #endregion IQueryHandler<GetDocumentQuery, DocumentDto> Members
    }
}