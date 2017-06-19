using System.Threading;
using System.Threading.Tasks;
using InfoFenix.Core.Cqrs;
using InfoFenix.Core.Dto;
using InfoFenix.Core.IO;
using InfoFenix.Core.Logging;
using InfoFenix.Core.PubSub;

namespace InfoFenix.Core.Commands {

    public class StopWatchDocumentDirectoryCommand : ICommand {

        #region Public Properties

        public DocumentDirectoryDto DocumentDirectory { get; set; }

        #endregion Public Properties
    }

    public class StopWatchDocumentDirectoryCommandHandler : ICommandHandler<StopWatchDocumentDirectoryCommand> {

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

        public StopWatchDocumentDirectoryCommandHandler(IDirectoryWatcherManager directoryWatcherManager, IPublisherSubscriber publisherSubscriber) {
            Prevent.ParameterNull(directoryWatcherManager, nameof(directoryWatcherManager));
            Prevent.ParameterNull(publisherSubscriber, nameof(publisherSubscriber));

            _directoryWatcherManager = directoryWatcherManager;
            _publisherSubscriber = publisherSubscriber;
        }

        #endregion Public Constructors

        #region ICommandHandler<StopWatchDocumentDirectoryCommand> Members

        public Task HandleAsync(StopWatchDocumentDirectoryCommand command, CancellationToken cancellationToken = default(CancellationToken)) {
            var actualStep = 0;
            var totalSteps = 1;

            return Task.Run(() => {
                _publisherSubscriber.ProgressiveTaskStartAsync(
                    title: Resources.Resources.StopWatchDocumentDirectory_ProgressiveTaskStart_Title,
                    actualStep: actualStep,
                    totalSteps: totalSteps
                );

                _publisherSubscriber.ProgressiveTaskPerformStepAsync(
                    message: string.Format(Resources.Resources.StopWatchDocumentDirectory_ProgressiveTaskPerformStep_Message, command.DocumentDirectory.Label),
                    actualStep: actualStep,
                    totalSteps: totalSteps
                );

                _directoryWatcherManager.StopWatch(command.DocumentDirectory.Path);
            }, cancellationToken)
            .ContinueWith(_publisherSubscriber.TaskContinuation, new ProgressiveTaskContinuationInfo {
                ActualStep = actualStep,
                TotalSteps = totalSteps,
                Log = Log
            });
        }

        #endregion ICommandHandler<StopWatchDocumentDirectoryCommand> Members
    }
}