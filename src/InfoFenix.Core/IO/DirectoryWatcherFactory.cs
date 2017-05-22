using InfoFenix.Core.IoC;

namespace InfoFenix.Core.IO {
    public sealed class DirectoryWatcherFactory : IDirectoryWatcherFactory {

        #region Private Read-Only Fields

        private readonly IResolver _resolver;

        #endregion Private Read-Only Fields

        #region Public Constructors

        public DirectoryWatcherFactory(IResolver resolver) {
            Prevent.ParameterNull(resolver, nameof(resolver));

            _resolver = resolver;
        }

        #endregion Public Constructors

        #region IDirectoryWatcherFactory Members

        public IDirectoryWatcher Create() {
            return _resolver.Resolve<IDirectoryWatcher>();
        }

        #endregion IDirectoryWatcherFactory Members
    }
}