using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using InfoFenix.Core.Cqrs;
using InfoFenix.Core.Data;
using InfoFenix.Core.Dto;
using InfoFenix.Core.Entities;
using SQL = InfoFenix.Core.Resources.Resources;

namespace InfoFenix.Core.Queries {

    public class ListDocumentsByDocumentDirectoryQuery : IQuery<IEnumerable<DocumentDto>> {

        #region Public Properties

        public int DocumentDirectoryID { get; set; }

        #endregion Public Properties
    }

    public class ListDocumentsByDocumentDirectoryQueryHandler : IQueryHandler<ListDocumentsByDocumentDirectoryQuery, IEnumerable<DocumentDto>> {

        #region Private Read-Only Fields

        private readonly IDatabase _database;

        #endregion Private Read-Only Fields

        #region Public Constructors

        public ListDocumentsByDocumentDirectoryQueryHandler(IDatabase database) {
            Prevent.ParameterNull(database, nameof(database));

            _database = database;
        }

        #endregion Public Constructors

        #region IQueryHandler<ListDocumentsByDocumentDirectoryQuery, IEnumerable<DocumentDto>> Members

        public Task<IEnumerable<DocumentDto>> HandleAsync(ListDocumentsByDocumentDirectoryQuery query, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => {
                return _database.ExecuteReader(SQL.ListDocumentsByDocumentDirectory, DocumentDto.Map, parameters: new[] {
                    Parameter.CreateInputParameter(nameof(DocumentEntity.DocumentDirectoryID), query.DocumentDirectoryID, DbType.Int32)
                });
            }, cancellationToken);
        }

        #endregion IQueryHandler<ListDocumentsByDocumentDirectoryQuery, IEnumerable<DocumentDto>> Members
    }
}