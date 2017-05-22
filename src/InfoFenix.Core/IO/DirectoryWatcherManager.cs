using System;
using System.Collections.Generic;

namespace InfoFenix.Core.IO {

    public sealed class DirectoryWatcherManager : IDirectoryWatcherManager, IDisposable {

        #region Private Static Ready-Only Fields

        private static readonly IDictionary<string, IDirectoryWatcher> Cache = new Dictionary<string, IDirectoryWatcher>(StringComparer.CurrentCultureIgnoreCase);
        private static readonly object SyncLock = new object();

        #endregion Private Static Ready-Only Fields

        #region Private Read-Only Fields

        private readonly IDirectoryWatcherFactory _factory;

        #endregion Private Read-Only Fields

        #region Private Fields

        private bool _disposed;

        #endregion Private Fields

        #region Public Constructors

        public DirectoryWatcherManager(IDirectoryWatcherFactory factory) {
            Prevent.ParameterNull(factory, nameof(factory));

            _factory = factory;
        }

        #endregion Public Constructors

        #region Destructor

        ~DirectoryWatcherManager() {
            Dispose(disposing: false);
        }

        #endregion Destructor

        #region Private Methods

        private void Dispose(bool disposing) {
            if (_disposed) { return; }
            if (disposing) {
                lock (SyncLock) {
                    foreach (var item in Cache.Values) {
                        var disposable = item as IDisposable;
                        if (disposable != null) {
                            disposable.Dispose();
                        }
                    }
                    Cache.Clear();
                }
            }
            _disposed = true;
        }

        #endregion Private Methods

        #region IDirectoryWatcherManager Members

        public void StartWatch(string path) {
            lock (SyncLock) {
                if (!Cache.ContainsKey(path)) {
                    Cache.Add(path, _factory.Create());
                    Cache[path].Watch(path);
                }
            }
        }

        public void StopWatch(string path) {
            lock (SyncLock) {
                if (Cache.ContainsKey(path)) {
                    var disposable = Cache[path] as IDisposable;
                    if (disposable != null) {
                        disposable.Dispose();
                    }
                    Cache.Remove(path);
                }
            }
        }

        #endregion IDirectoryWatcherManager Members

        #region IDisposable Members

        public void Dispose() {
            Dispose(disposing: true);
            GC.SuppressFinalize(obj: this);
        }

        #endregion IDisposable Members
    }
}