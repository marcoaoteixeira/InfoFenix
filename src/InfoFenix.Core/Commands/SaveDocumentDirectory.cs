using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using InfoFenix.Core.Cqrs;
using InfoFenix.Core.Data;
using InfoFenix.Core.Dto;
using InfoFenix.Core.Logging;
using InfoFenix.Core.PubSub;
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
        private readonly IPublisherSubscriber _publisherSuscriber;

        #endregion Private Read-Only Fields

        #region Public Properties

        private ILogger _log;

        public ILogger Log {
            get { return _log ?? NullLogger.Instance; }
            set { _log = value ?? NullLogger.Instance; }
        }

        #endregion Public Properties

        #region Public Constructors

        public SaveDocumentDirectoryCommandHandler(IDatabase database, IPublisherSubscriber publisherSuscriber) {
            Prevent.ParameterNull(database, nameof(database));
            Prevent.ParameterNull(publisherSuscriber, nameof(publisherSuscriber));

            _database = database;
            _publisherSuscriber = publisherSuscriber;
        }

        #endregion Public Constructors

        #region ICommandHandler<SaveDocumentDirectoryCommand> Members

        public Task HandleAsync(SaveDocumentDirectoryCommand command, CancellationToken cancellationToken = default(CancellationToken)) {
            var info = new ProgressiveTaskContinuationInfo {
                Log = Log,
                TotalSteps = 1
            };

            return Task.Run(() => {
                _publisherSuscriber.ProgressiveTaskStart(
                    title: Resource.SaveDocumentDirectory_ProgressiveTaskStart_Title,
                    actualStep: info.ActualStep,
                    totalSteps: info.TotalSteps
                );

                using (var transaction = _database.Connection.BeginTransaction()) {
                    _publisherSuscriber.ProgressiveTaskStart(
                        title: string.Format(Resource.SaveDocumentDirectory_ProgressiveTaskPerformStep_Message, command.DocumentDirectory.Label),
                        actualStep: ++info.ActualStep,
                        totalSteps: info.TotalSteps
                    );

                    var result = _database.ExecuteScalar(Resource.SaveDocumentDirectorySQL, parameters: new[] {
                        Parameter.CreateInputParameter(Common.DatabaseSchema.DocumentDirectories.DocumentDirectoryID, command.DocumentDirectory.DocumentDirectoryID != 0 ? (object)command.DocumentDirectory.DocumentDirectoryID : DBNull.Value, DbType.Int32),
                        Parameter.CreateInputParameter(Common.DatabaseSchema.DocumentDirectories.Label, command.DocumentDirectory.Label),
                        Parameter.CreateInputParameter(Common.DatabaseSchema.DocumentDirectories.Path, command.DocumentDirectory.Path),
                        Parameter.CreateInputParameter(Common.DatabaseSchema.DocumentDirectories.Code, command.DocumentDirectory.Code),
                        Parameter.CreateInputParameter(Common.DatabaseSchema.DocumentDirectories.Watch, command.DocumentDirectory.Watch ? 1 : 0, DbType.Int32),
                        Parameter.CreateInputParameter(Common.DatabaseSchema.DocumentDirectories.Index, command.DocumentDirectory.Index ? 1 : 0, DbType.Int32)
                    });
                    if (command.DocumentDirectory.DocumentDirectoryID <= 0) { command.DocumentDirectory.DocumentDirectoryID = Convert.ToInt32(result); }

                    cancellationToken.ThrowIfCancellationRequested();
                    transaction.Commit();
                }
            }, cancellationToken)
            .ContinueWith(_publisherSuscriber.TaskContinuation, info);
        }

        #endregion ICommandHandler<SaveDocumentDirectoryCommand> Members
    }
}