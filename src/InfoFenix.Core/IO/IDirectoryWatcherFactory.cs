namespace InfoFenix.Core.IO {

    public interface IDirectoryWatcherFactory {

        #region Methods

        IDirectoryWatcher Create();

        #endregion Methods
    }
}