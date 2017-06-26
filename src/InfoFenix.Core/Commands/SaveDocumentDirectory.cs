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

        #region Private Methods

        private void CheckCancellationTokenAndNotify(CancellationToken cancellationToken, int actualStep, int totalSteps) {
            if (cancellationToken.IsCancellationRequested) {
                _publisherSuscriber.Publish(new ProgressiveTaskCancelNotification {
                    Arguments = new ProgressiveTaskArguments {
                        ActualStep = actualStep,
                        TotalSteps = totalSteps
                    }
                });

                cancellationToken.ThrowIfCancellationRequested();
            }
        }

        #endregion Private Methods

        #region ICommandHandler<SaveDocumentDirectoryCommand> Members

        public Task HandleAsync(SaveDocumentDirectoryCommand command, IProgress<ProgressArguments> progress = null, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => {
                var actualStep = 0;
                var totalSteps = 1;

                _publisherSuscriber.Publish(new ProgressiveTaskStartNotification {
                    Arguments = new ProgressiveTaskArguments {
                        Title = Resource.SaveDocumentDirectory_ProgressiveTaskStart_Title,
                        ActualStep = actualStep,
                        TotalSteps = totalSteps
                    }
                });

                using (var transaction = _database.Connection.BeginTransaction()) {
                    try {
                        CheckCancellationTokenAndNotify(cancellationToken, actualStep, totalSteps);

                        _publisherSuscriber.Publish(new ProgressiveTaskPerformStepNotification {
                            Arguments = new ProgressiveTaskArguments {
                                Message = string.Format(Resource.SaveDocumentDirectory_ProgressiveTaskPerformStep_Message, command.DocumentDirectory.Label),
                                ActualStep = ++actualStep,
                                TotalSteps = totalSteps
                            }
                        });

                        var result = _database.ExecuteScalar(Resource.SaveDocumentDirectorySQL, parameters: new[] {
                            Parameter.CreateInputParameter(Common.DatabaseSchema.DocumentDirectories.DocumentDirectoryID, command.DocumentDirectory.DocumentDirectoryID != 0 ? (object)command.DocumentDirectory.DocumentDirectoryID : DBNull.Value, DbType.Int32),
                            Parameter.CreateInputParameter(Common.DatabaseSchema.DocumentDirectories.Label, command.DocumentDirectory.Label),
                            Parameter.CreateInputParameter(Common.DatabaseSchema.DocumentDirectories.Path, command.DocumentDirectory.Path),
                            Parameter.CreateInputParameter(Common.DatabaseSchema.DocumentDirectories.Code, command.DocumentDirectory.Code),
                            Parameter.CreateInputParameter(Common.DatabaseSchema.DocumentDirectories.Watch, command.DocumentDirectory.Watch ? 1 : 0, DbType.Int32),
                            Parameter.CreateInputParameter(Common.DatabaseSchema.DocumentDirectories.Index, command.DocumentDirectory.Index ? 1 : 0, DbType.Int32)
                        });
                        if (command.DocumentDirectory.DocumentDirectoryID <= 0) { command.DocumentDirectory.DocumentDirectoryID = Convert.ToInt32(result); }

                        CheckCancellationTokenAndNotify(cancellationToken, actualStep, totalSteps);
                        transaction.Commit();
                    } catch (Exception ex) {
                        _publisherSuscriber.Publish(new ProgressiveTaskErrorNotification {
                            Arguments = new ProgressiveTaskArguments {
                                ActualStep = actualStep,
                                TotalSteps = totalSteps,
                                Error = ex.Message
                            }
                        });

                        throw;
                    }
                }
            }, cancellationToken);
        }

        #endregion ICommandHandler<SaveDocumentDirectoryCommand> Members
    }
}