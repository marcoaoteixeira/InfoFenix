using System;
using System.Threading;
using System.Threading.Tasks;
using InfoFenix.CQRS;
using InfoFenix.Data;
using InfoFenix.Resources;

namespace InfoFenix.Domains.Queries {

    public enum ObjectType {
        Table,
        Index,
        View,
        Trigger,
        Query
    }

    public sealed class ObjectExistsQuery : IQuery<bool> {

        #region Public Properties

        public ObjectType Type { get; set; }

        public string Name { get; set; }

        #endregion Public Properties
    }

    public sealed class ObjectExistsQueryHandler : IQueryHandler<ObjectExistsQuery, bool> {

        #region Private Read-Only Fields

        private readonly IDatabase _database;

        #endregion Private Read-Only Fields

        #region Public Constructors

        public ObjectExistsQueryHandler(IDatabase database) {
            Prevent.ParameterNull(database, nameof(database));

            _database = database;
        }

        #endregion Public Constructors

        #region IQueryHandler<ObjectExistsQuery, bool> Members

        public Task<bool> HandleAsync(ObjectExistsQuery query, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => {
                var result = _database.ExecuteScalar(SQLs.ObjectExists, parameters: new[] {
                    Parameter.CreateInputParameter(Common.DatabaseSchema.SQLiteMaster.Type, query.Type.ToString().ToLowerInvariant()),
                    Parameter.CreateInputParameter(Common.DatabaseSchema.SQLiteMaster.Name, query.Name)
                });
                return Convert.ToBoolean(result);
            }, cancellationToken);
        }

        #endregion IQueryHandler<ObjectExistsQuery, bool> Members
    }
}