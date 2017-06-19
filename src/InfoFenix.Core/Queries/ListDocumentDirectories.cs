using System;
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

    public class ListDocumentDirectoriesQuery : IQuery<IEnumerable<DocumentDirectoryDto>> {

        #region Public Properties

        public string Label { get; set; }

        public string Path { get; set; }

        public string Code { get; set; }

        public bool? Watch { get; set; }

        public bool? Index { get; set; }

        #endregion Public Properties
    }

    public class ListDocumentDirectoriesQueryHandler : IQueryHandler<ListDocumentDirectoriesQuery, IEnumerable<DocumentDirectoryDto>> {

        #region Private Read-Only Fields

        private readonly IDatabase _database;

        #endregion Private Read-Only Fields

        #region Public Constructors

        public ListDocumentDirectoriesQueryHandler(IDatabase database) {
            Prevent.ParameterNull(database, nameof(database));

            _database = database;
        }

        #endregion Public Constructors

        #region IQueryHandler<ListDocumentDirectoriesQuery, IEnumerable<DocumentDirectoryDto>> Members

        public Task<IEnumerable<DocumentDirectoryDto>> HandleAsync(ListDocumentDirectoriesQuery query, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => {
                return _database.ExecuteReader(SQL.ListDocumentDirectories, DocumentDirectoryDto.Map, parameters: new[] {
                    Parameter.CreateInputParameter(nameof(DocumentDirectoryEntity.Label), (object)query.Label ?? DBNull.Value),
                    Parameter.CreateInputParameter(nameof(DocumentDirectoryEntity.Path), (object)query.Path ?? DBNull.Value),
                    Parameter.CreateInputParameter(nameof(DocumentDirectoryEntity.Code), (object)query.Code ?? DBNull.Value),
                    Parameter.CreateInputParameter(nameof(DocumentDirectoryEntity.Watch), query.Watch.HasValue ? (object)(query.Watch.Value ? 1 : 0) : DBNull.Value , DbType.Int32),
                    Parameter.CreateInputParameter(nameof(DocumentDirectoryEntity.Index), query.Index.HasValue ? (object)(query.Index.Value ? 1 : 0) : DBNull.Value , DbType.Int32)
                });
            }, cancellationToken);
        }

        #endregion IQueryHandler<ListDocumentDirectoriesQuery, IEnumerable<DocumentDirectoryDto>> Members
    }
}