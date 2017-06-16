using System;
using System.Threading;
using System.Threading.Tasks;
using InfoFenix.Core.Cqrs;
using InfoFenix.Core.IO;
using InfoFenix.Core.Logging;
using InfoFenix.Core.PubSub;

namespace InfoFenix.Core.Commands {

    public class StartWatchDocumentDirectoryCommand : ICommand {

        #region Public Properties

        public string DirectoryPath { get; set; }

        #endregion Public Properties
    }

    public class StartWatchDocumentDirectoryCommandHandler : ICommandHandler<StartWatchDocumentDirectoryCommand> {

        #region Private Read-Only Fields

        private readonly IDirectoryWatcherManager _directoryWatcherManager;
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

        public StartWatchDocumentDirectoryCommandHandler(IDirectoryWatcherManager directoryWatcherManager, IPublisherSubscriber publisherSubscriber) {
            Prevent.ParameterNull(directoryWatcherManager, nameof(directoryWatcherManager));
            Prevent.ParameterNull(publisherSubscriber, nameof(publisherSubscriber));

            _directoryWatcherManager = directoryWatcherManager;
            _publisherSubscriber = publisherSubscriber;
        }

        #endregion Public Constructors

        #region ICommandHandler<StartWatchDocumentDirectoryCommand> Members

        public Task HandleAsync(StartWatchDocumentDirectoryCommand command, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => {
                _publisherSubscriber.PublishAsync(new ProgressiveTaskStartNotification {
                    Title = Resources.Resources.StartWatchDocumentDirectory_ProgressiveTask_Title,
                    TotalSteps = 1
                });

                try {
                    _directoryWatcherManager.StartWatch(command.DirectoryPath);

                    _publisherSubscriber.PublishAsync(new ProgressiveTaskPerformStepNotification {
                        ActualStep = 1,
                        TotalSteps = 1
                    });
                } catch (Exception ex) {
                    _publisherSubscriber.PublishAsync(new ProgressiveTaskErrorNotification {
                        TotalSteps = 1
                    });

                    Log.Error(ex, ex.Message); throw;
                } finally {
                    _publisherSubscriber.PublishAsync(new ProgressiveTaskCompleteNotification {
                        TotalSteps = 1
                    });
                }
            }, cancellationToken);
        }

        #endregion ICommandHandler<StartWatchDocumentDirectoryCommand> Members
    }
}