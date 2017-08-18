using System;
using System.Threading;
using System.Threading.Tasks;
using InfoFenix.CQRS;
using InfoFenix.Data;
using InfoFenix.Logging;
using InfoFenix.Resources;

namespace InfoFenix.Migrations {

    public sealed class CreateMigrationTableSchemaCommand : ICommand {
    }

    public sealed class CreateMigrationTableSchemaCommandHandler : ICommandHandler<CreateMigrationTableSchemaCommand> {

        #region Private Read-Only Fields

        private readonly IDatabase _database;

        #endregion Private Read-Only Fields

        #region Public Properties

        private ILogger _log;

        public ILogger Log {
            get { return _log ?? NullLogger.Instance; }
            set { _log = value ?? NullLogger.Instance; }
        }

        #endregion Public Properties

        #region Public Constructors

        public CreateMigrationTableSchemaCommandHandler(IDatabase database) {
            Prevent.ParameterNull(database, nameof(database));

            _database = database;
        }

        #endregion Public Constructors

        #region ICommandHandler<CreateMigrationTableSchemaCommand> Members

        public Task HandleAsync(CreateMigrationTableSchemaCommand command, CancellationToken cancellationToken = default(CancellationToken), IProgress<ProgressInfo> progress = null) {
            return Task.Run(() => {
                try {
                    using (var transaction = _database.Connection.BeginTransaction()) {
                        _database.ExecuteNonQuery(SQLs.CreateMigrationTableSchema);
                        transaction.Commit();
                    }
                } catch { throw; }
            }, cancellationToken);
        }

        #endregion ICommandHandler<CreateMigrationTableSchemaCommand> Members
    }
}