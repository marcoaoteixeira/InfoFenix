using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using InfoFenix.Core.Cqrs;
using InfoFenix.Core.Data;
using InfoFenix.Core.Dto;
using InfoFenix.Core.Entities;
using InfoFenix.Core.Logging;
using InfoFenix.Core.PubSub;
using SQL = InfoFenix.Core.Resources.Resources;

using Strings = InfoFenix.Core.Resources.Resources;

namespace InfoFenix.Core.Commands {

    public sealed class RemoveDocumentDirectoryCommand : ICommand {

        #region Public Properties

        public DocumentDirectoryDto DocumentDirectory { get; set; }

        #endregion Public Properties
    }

    public sealed class RemoveDocumentDirectoryCommandHandler : ICommandHandler<RemoveDocumentDirectoryCommand> {

        #region Private Read-Only Fields

        private readonly IDatabase _database;
        private readonly IPublisherSubscriber _publisherSubscriber;

        #endregion Private Read-Only Fields

        #region Public Properties

        private ILogger _log;

        public ILogger Log {
            get { return _log ?? NullLogger.Instance; }
            set { _log = value ?? NullLogger.Instance; }
        }

        #endregion Public Properties

        #region Public Constructors

        public RemoveDocumentDirectoryCommandHandler(IDatabase database, IPublisherSubscriber publisherSubscriber) {
            Prevent.ParameterNull(database, nameof(database));
            Prevent.ParameterNull(publisherSubscriber, nameof(publisherSubscriber));

            _database = database;
            _publisherSubscriber = publisherSubscriber;
        }

        #endregion Public Constructors

        #region ICommandHandler<RemoveDocumentDirectoryCommand> Members

        public Task HandleAsync(RemoveDocumentDirectoryCommand command, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => {
                _publisherSubscriber.PublishAsync(new ProgressiveTaskStartNotification {
                    Title = Strings.RemoveDocumentDirectory_ProgressiveTask_Title,
                    TotalSteps = 1
                });

                try {
                    using (var transaction = _database.Connection.BeginTransaction()) {
                        _database.ExecuteScalar(SQL.RemoveDocumentDirectory, parameters: new[] {
                            Parameter.CreateInputParameter(nameof(DocumentDirectoryEntity.ID), command.DocumentDirectory.DocumentDirectoryID, DbType.Int32)
                        });

                        if (!cancellationToken.IsCancellationRequested) {
                            _publisherSubscriber.PublishAsync(new ProgressiveTaskPerformStepNotification {
                                ActualStep = 1,
                                TotalSteps = 1
                            });

                            transaction.Commit();
                        } else {
                            _publisherSubscriber.PublishAsync(new ProgressiveTaskCancelNotification {
                                TotalSteps = 1
                            });
                        }
                    }
                } catch (Exception ex) {
                    _publisherSubscriber.PublishAsync(new ProgressiveTaskErrorNotification {
                        Error = ex.Message,
                        TotalSteps = 1
                    });

                    Log.Error(ex, ex.Message);

                    throw;
                } finally {
                    _publisherSubscriber.PublishAsync(new ProgressiveTaskCompleteNotification {
                        TotalSteps = 1
                    });
                }
            }, cancellationToken);
        }

        #endregion ICommandHandler<RemoveDocumentDirectoryCommand> Members
    }
}