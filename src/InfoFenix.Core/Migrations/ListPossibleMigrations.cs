using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using InfoFenix.Core.CQRS;
using InfoFenix.Core.Data;

namespace InfoFenix.Core.Migrations {

    public sealed class ListPossibleMigrationsQuery : IQuery<IEnumerable<string>> {
    }

    public sealed class ListPossibleMigrationsQueryHandler : IQueryHandler<ListPossibleMigrationsQuery, IEnumerable<string>> {

        #region Private Read-Only Fields

        private readonly IDatabase _database;

        #endregion Private Read-Only Fields

        #region Public Constructors

        public ListPossibleMigrationsQueryHandler(IDatabase database) {
            Prevent.ParameterNull(database, nameof(database));

            _database = database;
        }

        #endregion Public Constructors

        #region IQueryHandler<ListPossibleMigrationsQuery, IEnumerable<string>> Members

        public Task<IEnumerable<string>> HandleAsync(ListPossibleMigrationsQuery query, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => {
                return Common
                    .GetManifestResourceNames()
                    .Where(_ => _.StartsWith(Common.MIGRATION_FOLDER))
                    .Select(_ => Regex.Match(_, @"\d+").Value)
                    .Distinct();
            }, cancellationToken);
        }

        #endregion IQueryHandler<ListPossibleMigrationsQuery, IEnumerable<string>> Members
    }
}