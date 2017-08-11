using System;
using System.Threading;
using System.Threading.Tasks;
using InfoFenix.Core.CQRS;
using InfoFenix.Core.Data;
using Resource = InfoFenix.Core.Resources.Resources;

namespace InfoFenix.Core.Migrations {

    public sealed class SaveMigrationCommand : ICommand {

        #region Public Properties

        public string Version { get; set; }
        public DateTime Date { get; set; }

        #endregion Public Properties
    }

    public sealed class SaveMigrationCommandHandler : ICommandHandler<SaveMigrationCommand> {

        #region Private Read-Only Fields

        private readonly IDatabase _database;

        #endregion Private Read-Only Fields

        #region Public Constructors

        public SaveMigrationCommandHandler(IDatabase database) {
            Prevent.ParameterNull(database, nameof(database));

            _database = database;
        }

        #endregion Public Constructors

        #region ICommandHandler<SaveMigrationCommand> Members

        public Task HandleAsync(SaveMigrationCommand command, CancellationToken cancellationToken = default(CancellationToken), IProgress<ProgressInfo> progress = null) {
            return Task.Run(() => {
                using (var transaction = _database.Connection.BeginTransaction()) {
                    try {
                        cancellationToken.ThrowIfCancellationRequested();
                        _database.ExecuteNonQuery(Resource.SaveMigrationSQL, parameters: new[] {
                            Parameter.CreateInputParameter(Common.DatabaseSchema.Migrations.Version, command.Version),
                            Parameter.CreateInputParameter(Common.DatabaseSchema.Migrations.Date, command.Date)
                        });
                        cancellationToken.ThrowIfCancellationRequested();
                        transaction.Commit();
                    } catch { transaction.Rollback(); throw; }
                }
            }, cancellationToken);
        }

        #endregion ICommandHandler<SaveMigrationCommand> Members
    }
}