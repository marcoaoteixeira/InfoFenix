using System.IO;
using InfoFenix.Core.Logging;

namespace InfoFenix.Core.Office {

    public sealed class SpireDocWordApplication : IWordApplication {

        #region Public Properties

        private ILogger _log;

        public ILogger Log {
            get { return _log ?? NullLogger.Instance; }
            set { _log = value ?? NullLogger.Instance; }
        }

        #endregion Public Properties

        #region IWordApplication Members

        public IWordDocument Open(Stream stream) {
            Prevent.ParameterNull(stream, nameof(stream));

            return new SpireDocWordDocument(stream, _log);
        }

        public IWordDocument Open(string filePath) {
            return Open(File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
        }

        public void CloseAllDocuments() {
        }

        public void Quit() {
        }

        #endregion IWordApplication Members
    }
}