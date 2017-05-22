using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using InfoFenix.Core.Logging;

namespace InfoFenix.Core {
    public class CancellationTokenIssuer : IDisposable {

        #region Private Read-Only Fields

        private readonly ConcurrentDictionary<string, CancellationTokenSource> _cache = new ConcurrentDictionary<string, CancellationTokenSource>();

        #endregion Private Read-Only Fields

        #region Private Fields

        private bool _disposed;

        #endregion Private Fields

        #region Public Properties

        private ILogger _log;

        public ILogger Log {
            get { return _log ?? NullLogger.Instance; }
            set { _log = value ?? NullLogger.Instance; }
        }

        #endregion Public Properties

        #region Destructor

        ~CancellationTokenIssuer() {
            Dispose(disposing: false);
        }

        #endregion Destructor

        #region Public Methods

        public CancellationToken Get(string key) {
            return _cache.GetOrAdd(key, _ => new CancellationTokenSource()).Token;
        }

        public void MarkAsComplete(string key) {
            CancellationTokenSource source;
            if (_cache.TryRemove(key, out source)) {
                try { source.Dispose(); } catch (Exception ex) { Log.Error(ex, ex.Message); }
            }
        }

        public void Cancel(string key) {
            CancellationTokenSource source;
            if (_cache.TryRemove(key, out source)) {
                try { source.Cancel(); } catch (Exception ex) { Log.Error(ex, ex.Message); }
            }
        }

        public void CancelAll() {
            _cache.Keys.ToArray().Each(Cancel);
        }

        #endregion Public Methods

        #region Private Methods

        private void Dispose(bool disposing) {
            if (_disposed) { return; }
            if (disposing) {
                _cache.Keys.Each(_ => {
                    CancellationTokenSource source;
                    if (_cache.TryGetValue(_, out source)) {
                        source.Dispose();
                    }
                });
            }
            _disposed = true;
        }

        #endregion Private Methods

        #region IDisposable Members

        public void Dispose() {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable Members
    }
}