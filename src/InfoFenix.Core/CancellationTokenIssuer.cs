using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using InfoFenix.Core.Logging;

namespace InfoFenix.Core {

    public sealed class CancellationTokenIssuer : IDisposable {

        #region Private Fields

        private ConcurrentDictionary<string, CancellationTokenSource> _cache;
        private bool _disposed;

        #endregion Private Fields

        #region Public Properties

        private ILogger _log;

        public ILogger Log {
            get { return _log ?? NullLogger.Instance; }
            set { _log = value ?? NullLogger.Instance; }
        }

        #endregion Public Properties

        #region Public Constructors

        public CancellationTokenIssuer() {
            _cache = new ConcurrentDictionary<string, CancellationTokenSource>();
        }

        #endregion Public Constructors

        #region Destructor

        ~CancellationTokenIssuer() {
            Dispose(disposing: false);
        }

        #endregion Destructor

        #region Public Methods

        public CancellationToken Get(string key) {
            PreventCallAfterDispose();
            return _cache.GetOrAdd(key, _ => new CancellationTokenSource()).Token;
        }

        public void MarkAsComplete(string key) {
            PreventCallAfterDispose();
            if (_cache.TryRemove(key, out CancellationTokenSource source)) {
                try { source.Dispose(); } catch (Exception ex) { Log.Error(ex.Message); }
            }
        }

        public bool Cancel(string key) {
            PreventCallAfterDispose();
            if (_cache.TryRemove(key, out CancellationTokenSource source)) {
                try { source.Cancel(); return true; }
                catch (Exception ex) { Log.Error(ex.Message); }
            }
            return false;
        }

        public void CancelAll() {
            PreventCallAfterDispose();
            var keys = _cache.Keys.ToArray();
            foreach (var key in keys) {
                Cancel(key);
            }
        }

        #endregion Public Methods

        #region Private Methods

        private void Dispose(bool disposing) {
            if (_disposed) { return; }
            if (disposing) { CancelAll(); }
            _disposed = true;
        }

        private void PreventCallAfterDispose() {
            if (_disposed) {
                throw new ObjectDisposedException(nameof(CancellationTokenIssuer));
            }
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