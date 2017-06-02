using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using InfoFenix.Core.Logging;

namespace InfoFenix.Core.Office {

    public sealed class SpireDocWordApplication : IWordApplication {

        #region Private Fields

        private readonly ConcurrentDictionary<int, SpireDocWordDocument> _cache;

        #endregion Private Fields

        #region Public Properties

        private ILogger _log;

        public ILogger Log {
            get { return _log ?? NullLogger.Instance; }
            set { _log = value ?? NullLogger.Instance; }
        }

        #endregion Public Properties

        #region Public Constructors

        public SpireDocWordApplication() {
            _cache = new ConcurrentDictionary<int, SpireDocWordDocument>();
        }

        #endregion Public Constructors

        #region Private Methods

        private void RemoveInstance(SpireDocWordDocument document) {
            _cache.TryRemove(document.Key, out SpireDocWordDocument value);
        }

        #endregion Private Methods

        #region IWordApplication Members

        public IWordDocument Open(Stream stream) {
            Prevent.ParameterNull(stream, nameof(stream));

            IWordDocument document = null;
            try {
                document = _cache.GetOrAdd(stream.GetHashCode(), key => {
                    var result = new SpireDocWordDocument(stream, _log);
                    result.Closed += RemoveInstance;
                    return result;
                });
            }
            catch (Exception ex) { Log.Error(ex, ex.Message); }
            finally { document = document ?? NullWordDocument.Instance; }
            return document;
        }

        public IWordDocument Open(string filePath) {
            return Open(File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
        }

        public void Quit() {
            foreach (var value in _cache.Values.ToArray()) {
                value.Dispose();
            }
        }

        #endregion IWordApplication Members
    }
}