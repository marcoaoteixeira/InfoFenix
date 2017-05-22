namespace InfoFenix.Core.IO {

    public sealed class NullDirectoryWatcherManager : IDirectoryWatcherManager {

        #region Public Static Read-Only Fields

        public static readonly IDirectoryWatcherManager Instance = new NullDirectoryWatcherManager();

        #endregion Public Static Read-Only Fields

        #region Private Constructors

        private NullDirectoryWatcherManager() {
        }

        #endregion Private Constructors

        #region IDirectoryWatcherManager Members

        public void StartWatch(string path) {
        }

        public void StopWatch(string path) {

        }

        #endregion IDirectoryWatcherManager Members
    }
}