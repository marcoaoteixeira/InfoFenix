using System;
using System.Threading;
using System.Threading.Tasks;
using InfoFenix;
using InfoFenix.CQRS;
using InfoFenix.Data;
using InfoFenix.Resources;

namespace InfoFenix.Domains.Commands {

    public sealed class DatabaseCleanUpCommand : ICommand {
    }

    public sealed class DatabaseCleanUpCommandHandler : ICommandHandler<DatabaseCleanUpCommand> {

        #region Private Read-Only Fields

        private readonly IDatabase _database;

        #endregion Private Read-Only Fields

        #region Public Constructors

        public DatabaseCleanUpCommandHandler(IDatabase database) {
            Prevent.ParameterNull(database, nameof(database));

            _database = database;
        }

        #endregion Public Constructors

        #region ICommandHandler<DatabaseCleanUpCommand> Members

        public Task HandleAsync(DatabaseCleanUpCommand command, CancellationToken cancellationToken = default(CancellationToken), IProgress<ProgressInfo> progress = null) {
            return Task.Run(() => {
                var actualStep = 0;
                var totalSteps = 1;
                try {
                    progress.Start(totalSteps, Strings.DatabaseCleanUp_Progress_Start_Title);

                    if (cancellationToken.IsCancellationRequested) {
                        progress.Cancel(actualStep, totalSteps);

                        cancellationToken.ThrowIfCancellationRequested();
                    }

                    progress.PerformStep(++actualStep, totalSteps, Strings.DatabaseCleanUp_Progress_Step_Database_Message);

                    _database.ExecuteNonQuery("VACUUM;");

                    progress.Complete(actualStep, totalSteps);
                } catch (Exception ex) { progress.Error(actualStep, totalSteps, ex.Message); throw; }
            }, cancellationToken);
        }

        #endregion ICommandHandler<DatabaseCleanUpCommand> Members
    }
}