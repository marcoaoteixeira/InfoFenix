using InfoFenix.Core.Cqrs;
using InfoFenix.Core.IO;
using InfoFenix.Core.Queries;

namespace InfoFenix.Core.Bootstrap.Actions {

    [Order(4)]
    public class WatchDocumentDirectoriesAction : ActionBase {

        #region Private Read-Only Fields

        private readonly IDirectoryWatcherManager _directoryWatcherManager;
        private readonly IDispatcher _dispatcher;

        #endregion Private Read-Only Fields

        #region Public Constructors

        public WatchDocumentDirectoriesAction(IDirectoryWatcherManager directoryWatcherManager, IDispatcher dispatcher) {
            Prevent.ParameterNull(directoryWatcherManager, nameof(directoryWatcherManager));
            Prevent.ParameterNull(dispatcher, nameof(dispatcher));

            _directoryWatcherManager = directoryWatcherManager;
            _dispatcher = dispatcher;
        }

        #endregion Public Constructors

        #region IAction Members

        public override void Execute() {
            var documentDirectories = _dispatcher.Query(new ListDocumentDirectoriesQuery());
            foreach (var documentDirectory in documentDirectories) {
                _directoryWatcherManager.StartWatch(documentDirectory.DirectoryPath);
            }
        }

        #endregion IAction Members
    }
}