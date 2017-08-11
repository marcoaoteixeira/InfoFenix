using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using InfoFenix.Core.CQRS;
using InfoFenix.Core.Data;
using InfoFenix.Core.Entities;
using Resource = InfoFenix.Core.Resources.Resources;

namespace InfoFenix.Core.Queries {

    public class ListDocumentDirectoriesQuery : IQuery<IEnumerable<DocumentDirectory>> {

        #region Public Properties

        public string Label { get; set; }

        public string Path { get; set; }

        public string Code { get; set; }

        #endregion Public Properties
    }

    public class ListDocumentDirectoriesQueryHandler : IQueryHandler<ListDocumentDirectoriesQuery, IEnumerable<DocumentDirectory>> {

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

        public Task<IEnumerable<DocumentDirectory>> HandleAsync(ListDocumentDirectoriesQuery query, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => {
                return _database.ExecuteReader(Resource.ListDocumentDirectoriesSQL, DocumentDirectory.Map, parameters: new[] {
                    Parameter.CreateInputParameter(Common.DatabaseSchema.DocumentDirectories.Code, (object)query.Code ?? DBNull.Value),
                    Parameter.CreateInputParameter(Common.DatabaseSchema.DocumentDirectories.Label, (object)query.Label ?? DBNull.Value),
                    Parameter.CreateInputParameter(Common.DatabaseSchema.DocumentDirectories.Path, (object)query.Path ?? DBNull.Value)
                });
            }, cancellationToken);
        }

        #endregion IQueryHandler<ListDocumentDirectoriesQuery, IEnumerable<DocumentDirectoryDto>> Members
    }
}