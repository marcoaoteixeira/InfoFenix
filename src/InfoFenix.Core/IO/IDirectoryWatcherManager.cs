namespace InfoFenix.Core.IO {
    public interface IDirectoryWatcherManager {

        #region Methods

        void StartWatch(string path);

        void StopWatch(string path);

        #endregion Methods
    }
}