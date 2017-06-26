using System;
using System.Threading;
using System.Threading.Tasks;
using InfoFenix.Core.Cqrs;
using InfoFenix.Core.Dto;
using InfoFenix.Core.IO;
using InfoFenix.Core.Logging;
using InfoFenix.Core.PubSub;
using Resource = InfoFenix.Core.Resources.Resources;

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

        public Task HandleAsync(StopWatchDocumentDirectoryCommand command, IProgress<ProgressArguments> progress = null, CancellationToken cancellationToken = default(CancellationToken)) {
            var info = new ProgressiveTaskContinuationInfo {
                Log = Log,
                TotalSteps = 1
            };

            return Task.Run(() => {
                _publisherSubscriber.ProgressiveTaskStart(
                    title: Resource.StopWatchDocumentDirectory_ProgressiveTaskStart_Title,
                    actualStep: info.ActualStep,
                    totalSteps: info.TotalSteps
                );

                _publisherSubscriber.ProgressiveTaskPerformStep(
                    message: string.Format(Resource.StopWatchDocumentDirectory_ProgressiveTaskPerformStep_Message, command.DocumentDirectory.Label),
                    actualStep: ++info.ActualStep,
                    totalSteps: info.TotalSteps
                );

                cancellationToken.ThrowIfCancellationRequested();
                _directoryWatcherManager.StopWatch(command.DocumentDirectory.Path);
            }, cancellationToken)
            .ContinueWith(_publisherSubscriber.TaskContinuation, info);
        }

        #endregion ICommandHandler<StopWatchDocumentDirectoryCommand> Members
    }
}