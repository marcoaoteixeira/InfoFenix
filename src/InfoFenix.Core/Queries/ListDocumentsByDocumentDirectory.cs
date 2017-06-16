using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using InfoFenix.Core;
using InfoFenix.Core.Cqrs;
using InfoFenix.Core.Data;
using InfoFenix.Core.Entities;
using SQL = InfoFenix.Core.Resources.Resources;

namespace InfoFenix.Core.Queries {
    public class ListDocumentsByDocumentDirectoryQuery : IQuery<IEnumerable<DocumentEntity>> {

        #region Public Properties

        public int DocumentDirectoryID { get; set; }

        #endregion Public Properties
    }

    public class ListDocumentsByDocumentDirectoryQueryHandler : IQueryHandler<ListDocumentsByDocumentDirectoryQuery, IEnumerable<DocumentEntity>> {

        #region Private Read-Only Fields

        private readonly IDatabase _database;

        #endregion Private Read-Only Fields

        #region Public Constructors

        public ListDocumentsByDocumentDirectoryQueryHandler(IDatabase database) {
            Prevent.ParameterNull(database, nameof(database));

            _database = database;
        }

        #endregion Public Constructors

        #region IQueryHandler<ListDocumentsByDocumentDirectoryQuery, IEnumerable<DocumentEntity>> Members

        public Task<IEnumerable<DocumentEntity>> HandleAsync(ListDocumentsByDocumentDirectoryQuery query, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => {
                return _database.ExecuteReader(SQL.ListDocumentsByDocumentDirectory, DocumentEntity.MapFromDataReader, parameters: new[] {
                    Parameter.CreateInputParameter(nameof(query.DocumentDirectoryID), query.DocumentDirectoryID, DbType.Int32)
                });
            }, cancellationToken);
        }

        #endregion IQueryHandler<ListDocumentsByDocumentDirectoryQuery, IEnumerable<DocumentEntity>> Members
    }
}