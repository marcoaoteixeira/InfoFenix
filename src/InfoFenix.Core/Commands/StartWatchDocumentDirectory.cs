using System;
using System.Threading;
using System.Threading.Tasks;
using InfoFenix.Core.Cqrs;
using InfoFenix.Core.Dto;
using InfoFenix.Core.IO;
using InfoFenix.Core.Logging;
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

        #endregion Private Read-Only Fields

        #region Public Properties

        private ILogger _log;

        public ILogger Log {
            get { return _log ?? NullLogger.Instance; }
            set { _log = value ?? NullLogger.Instance; }
        }

        #endregion Public Properties

        #region Public Constructors

        public StartWatchDocumentDirectoryCommandHandler(IDirectoryWatcherManager directoryWatcherManager) {
            Prevent.ParameterNull(directoryWatcherManager, nameof(directoryWatcherManager));

            _directoryWatcherManager = directoryWatcherManager;
        }

        #endregion Public Constructors

        #region ICommandHandler<StartWatchDocumentDirectoryCommand> Members

        public Task HandleAsync(StartWatchDocumentDirectoryCommand command, CancellationToken cancellationToken = default(CancellationToken), IProgress<ProgressInfo> progress = null) {
            return Task.Run(() => {
                var actualStep = 0;
                var totalSteps = 1;

                try {
                    progress.Start(totalSteps, Resource.StartWatchDocumentDirectory_Progress_Start_Title);
                    progress.PerformStep(++actualStep, totalSteps, Resource.StartWatchDocumentDirectory_Progress_Step_Message, command.DocumentDirectory.Label);

                    if (cancellationToken.IsCancellationRequested) {
                        progress.Cancel(actualStep, totalSteps);

                        cancellationToken.ThrowIfCancellationRequested();
                    }

                    _directoryWatcherManager.StartWatch(command.DocumentDirectory.Path);

                    progress.Complete(actualStep, totalSteps);
                } catch (Exception ex) { progress.Error(actualStep, totalSteps, ex.Message); throw; }
            }, cancellationToken);
        }

        #endregion ICommandHandler<StartWatchDocumentDirectoryCommand> Members
    }
}