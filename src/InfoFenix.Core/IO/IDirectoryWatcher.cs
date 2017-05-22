namespace InfoFenix.Core.IO {

    public interface IDirectoryWatcher {

        #region Methods

        void Watch(string path);

        #endregion Methods
    }
}