using System;
using System.IO;
using Gnostice.Documents;
using InfoFenix.Core.Logging;

namespace InfoFenix.Core.Office {

    public sealed class GnosticeWordApplication : IWordApplication {

        #region Private Fields

        private readonly DocumentManager _manager;

        #endregion Private Fields

        #region Public Properties

        private ILogger _log;

        public ILogger Log {
            get { return _log ?? NullLogger.Instance; }
            set { _log = value ?? NullLogger.Instance; }
        }

        #endregion Public Properties

        #region Public Constructors

        public GnosticeWordApplication() {
            _manager = new DocumentManager();
        }

        #endregion Public Constructors

        #region Private Methods

        private void RemoveInstance(GnosticeWordDocument document) {
            _manager.CloseDocument(document.Document);
        }

        #endregion Private Methods

        #region IWordApplication Members

        public IWordDocument Open(Stream stream) {
            Prevent.ParameterNull(stream, nameof(stream));

            IWordDocument document = null;
            try {
                document = new GnosticeWordDocument(_manager.LoadDocument(stream), Log);
                ((GnosticeWordDocument)document).Closed += RemoveInstance;
            } catch (Exception ex) { Log.Error(ex, ex.Message); } finally { document = document ?? NullWordDocument.Instance; }
            return document;
        }

        public IWordDocument Open(string filePath) {
            return Open(File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
        }

        public void Quit() {
            _manager.CloseAllDocuments();
        }

        #endregion IWordApplication Members
    }
}