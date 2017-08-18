using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using InfoFenix.CQRS;
using InfoFenix.Data;
using InfoFenix.Resources;

namespace InfoFenix.Migrations {

    public sealed class ListAppliedMigrationsQuery : IQuery<IEnumerable<Migration>> {

        #region Public Properties

        public string Version { get; set; }

        #endregion Public Properties
    }

    public sealed class ListAppliedMigrationsQueryHandler : IQueryHandler<ListAppliedMigrationsQuery, IEnumerable<Migration>> {

        #region Private Read-Only Fields

        private readonly IDatabase _database;

        #endregion Private Read-Only Fields

        #region Public Constructors

        public ListAppliedMigrationsQueryHandler(IDatabase database) {
            Prevent.ParameterNull(database, nameof(database));

            _database = database;
        }

        #endregion Public Constructors

        #region IQueryHandler<ListAppliedMigrationsQuery, IEnumerable<Migration>> Members

        public Task<IEnumerable<Migration>> HandleAsync(ListAppliedMigrationsQuery query, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => {
                return _database.ExecuteReader(SQLs.ListAppliedMigrations, Migration.Map, parameters: new[] {
                    Parameter.CreateInputParameter(Common.DatabaseSchema.Migrations.Version, query.Version != null ? (object)query.Version: DBNull.Value)
                });
            }, cancellationToken);
        }

        #endregion IQueryHandler<ListAppliedMigrationsQuery, IEnumerable<Migration>> Members
    }
}