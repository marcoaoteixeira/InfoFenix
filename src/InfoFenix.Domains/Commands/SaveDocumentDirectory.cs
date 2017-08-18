using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using InfoFenix;
using InfoFenix.CQRS;
using InfoFenix.Data;
using InfoFenix.Logging;
using InfoFenix.Resources;

namespace InfoFenix.Domains.Commands {

    public sealed class SaveDocumentDirectoryCommand : ICommand {

        #region Public Properties

        public int DocumentDirectoryID { get; set; }
        public string Code { get; set; }
        public string Label { get; set; }
        public string Path { get; set; }
        public int Position { get; set; }

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
                    progress.Start(totalSteps, Strings.SaveDocumentDirectory_Progress_Start_Title);

                    using (var transaction = _database.Connection.BeginTransaction()) {
                        if (cancellationToken.IsCancellationRequested) {
                            progress.Cancel(actualStep, totalSteps);

                            cancellationToken.ThrowIfCancellationRequested();
                        }

                        progress.PerformStep(++actualStep, totalSteps, Strings.SaveDocumentDirectory_Progress_Step_Message, command.Label);

                        var result = _database.ExecuteScalar(SQLs.SaveDocumentDirectory, parameters: new[] {
                            Parameter.CreateInputParameter(Common.DatabaseSchema.DocumentDirectories.DocumentDirectoryID, command.DocumentDirectoryID != 0 ? (object)command.DocumentDirectoryID : DBNull.Value, DbType.Int32),
                            Parameter.CreateInputParameter(Common.DatabaseSchema.DocumentDirectories.Code, command.Code),
                            Parameter.CreateInputParameter(Common.DatabaseSchema.DocumentDirectories.Label, command.Label),
                            Parameter.CreateInputParameter(Common.DatabaseSchema.DocumentDirectories.Path, command.Path),
                            Parameter.CreateInputParameter(Common.DatabaseSchema.DocumentDirectories.Position, command.Position, DbType.Int32)
                        });
                        if (command.DocumentDirectoryID <= 0) { command.DocumentDirectoryID = Convert.ToInt32(result); }

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