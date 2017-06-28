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

        #endregion Private Read-Only Fields

        #region Public Properties

        private ILogger _log;

        public ILogger Log {
            get { return _log ?? NullLogger.Instance; }
            set { _log = value ?? NullLogger.Instance; }
        }

        #endregion Public Properties

        #region Public Constructors

        public StopWatchDocumentDirectoryCommandHandler(IDirectoryWatcherManager directoryWatcherManager) {
            Prevent.ParameterNull(directoryWatcherManager, nameof(directoryWatcherManager));

            _directoryWatcherManager = directoryWatcherManager;
        }

        #endregion Public Constructors

        #region ICommandHandler<StopWatchDocumentDirectoryCommand> Members

        public Task HandleAsync(StopWatchDocumentDirectoryCommand command, CancellationToken cancellationToken = default(CancellationToken), IProgress<ProgressInfo> progress = null) {
            return Task.Run(() => {
                var actualStep = 0;
                var totalSteps = 1;

                try {
                    progress.Start(totalSteps, Resource.StopWatchDocumentDirectory_Progress_Start_Title);
                    progress.PerformStep(++actualStep, totalSteps, Resource.StopWatchDocumentDirectory_Progress_Step_Message, command.DocumentDirectory.Label);

                    if (cancellationToken.IsCancellationRequested) {
                        progress.Cancel(actualStep, totalSteps);

                        cancellationToken.ThrowIfCancellationRequested();
                    }

                    _directoryWatcherManager.StopWatch(command.DocumentDirectory.Path);

                    progress.Complete(actualStep, totalSteps);
                } catch (Exception ex) { progress.Error(actualStep, totalSteps, ex.Message); throw; }
            }, cancellationToken);
        }

        #endregion ICommandHandler<StopWatchDocumentDirectoryCommand> Members
    }
}