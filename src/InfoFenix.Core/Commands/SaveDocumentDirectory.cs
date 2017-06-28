using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using InfoFenix.Core.Cqrs;
using InfoFenix.Core.Data;
using InfoFenix.Core.Dto;
using InfoFenix.Core.Logging;
using Resource = InfoFenix.Core.Resources.Resources;

namespace InfoFenix.Core.Commands {

    public sealed class SaveDocumentDirectoryCommand : ICommand {

        #region Public Properties

        public DocumentDirectoryDto DocumentDirectory { get; set; }

        #endregion Public Properties
    }

    public sealed class SaveDocumentDirectoryCommandHandler : ICommandHandler<SaveDocumentDirectoryCommand> {

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

        public SaveDocumentDirectoryCommandHandler(IDatabase database) {
            Prevent.ParameterNull(database, nameof(database));

            _database = database;
        }

        #endregion Public Constructors

        #region ICommandHandler<SaveDocumentDirectoryCommand> Members

        public Task HandleAsync(SaveDocumentDirectoryCommand command, CancellationToken cancellationToken = default(CancellationToken), IProgress<ProgressInfo> progress = null) {
            return Task.Run(() => {
                var actualStep = 0;
                var totalSteps = 1;

                try {
                    progress.Start(totalSteps, Resource.SaveDocumentDirectory_Progress_Start_Title);

                    using (var transaction = _database.Connection.BeginTransaction()) {
                        if (cancellationToken.IsCancellationRequested) {
                            progress.Cancel(actualStep, totalSteps);

                            cancellationToken.ThrowIfCancellationRequested();
                        }

                        progress.PerformStep(++actualStep, totalSteps, Resource.SaveDocumentDirectory_Progress_Step_Message, command.DocumentDirectory.Label);

                        var result = _database.ExecuteScalar(Resource.SaveDocumentDirectorySQL, parameters: new[] {
                            Parameter.CreateInputParameter(Common.DatabaseSchema.DocumentDirectories.DocumentDirectoryID, command.DocumentDirectory.DocumentDirectoryID != 0 ? (object)command.DocumentDirectory.DocumentDirectoryID : DBNull.Value, DbType.Int32),
                            Parameter.CreateInputParameter(Common.DatabaseSchema.DocumentDirectories.Label, command.DocumentDirectory.Label),
                            Parameter.CreateInputParameter(Common.DatabaseSchema.DocumentDirectories.Path, command.DocumentDirectory.Path),
                            Parameter.CreateInputParameter(Common.DatabaseSchema.DocumentDirectories.Code, command.DocumentDirectory.Code),
                            Parameter.CreateInputParameter(Common.DatabaseSchema.DocumentDirectories.Watch, command.DocumentDirectory.Watch ? 1 : 0, DbType.Int32),
                            Parameter.CreateInputParameter(Common.DatabaseSchema.DocumentDirectories.Index, command.DocumentDirectory.Index ? 1 : 0, DbType.Int32)
                        });
                        if (command.DocumentDirectory.DocumentDirectoryID <= 0) { command.DocumentDirectory.DocumentDirectoryID = Convert.ToInt32(result); }

                        if (cancellationToken.IsCancellationRequested) {
                            progress.Cancel(actualStep, totalSteps);

                            cancellationToken.ThrowIfCancellationRequested();
                        }
                        transaction.Commit();

                        progress.Complete(actualStep, totalSteps);
                    }
                } catch (Exception ex) { progress.Cancel(actualStep, totalSteps, ex.Message); throw; }
            }, cancellationToken);
        }

        #endregion ICommandHandler<SaveDocumentDirectoryCommand> Members
    }
}