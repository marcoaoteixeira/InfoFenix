using System.Threading;
using System.Threading.Tasks;
using InfoFenix.Core.Cqrs;
using InfoFenix.Core.Dto;
using InfoFenix.Core.IO;
using InfoFenix.Core.Logging;
using InfoFenix.Core.PubSub;
using Resource = InfoFenix.Core.Resources.Resources;

namespace InfoFenix.Core.Commands {

    public class StartWatchDocumentDirectoryCommand : ICommand {

        #region Public Properties

        public DocumentDirectoryDto DocumentDirectory { get; set; }

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
            var info = new ProgressiveTaskContinuationInfo {
                Log = Log,
                TotalSteps = 1
            };

            return Task.Run(() => {
                _publisherSubscriber.ProgressiveTaskStart(
                    title: Resource.StartWatchDocumentDirectory_ProgressiveTaskStart_Title,
                    actualStep: info.ActualStep,
                    totalSteps: info.TotalSteps
                );

                _publisherSubscriber.ProgressiveTaskPerformStep(
                    message: string.Format(Resource.StartWatchDocumentDirectory_ProgressiveTaskPerformStep_Message, command.DocumentDirectory.Label),
                    actualStep: ++info.ActualStep,
                    totalSteps: info.TotalSteps
                );

                cancellationToken.ThrowIfCancellationRequested();
                _directoryWatcherManager.StartWatch(command.DocumentDirectory.Path);
            }, cancellationToken)
            .ContinueWith(_publisherSubscriber.TaskContinuation, info);
        }

        #endregion ICommandHandler<StartWatchDocumentDirectoryCommand> Members
    }
}