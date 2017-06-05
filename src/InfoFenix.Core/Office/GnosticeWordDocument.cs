using System;
using Gnostice.Documents;
using InfoFenix.Core.Logging;
using Spire.Doc;

namespace InfoFenix.Core.Office {

    public sealed class GnosticeWordDocument : IWordDocument, IDisposable {

        #region Private Read-Only Fields

        private readonly IDocument _document;
        private readonly ILogger _log;

        #endregion Private Read-Only Fields

        #region Public Events

        public event Action<GnosticeWordDocument> Closed;

        #endregion Public Events

        #region Public Properties

        public IDocument Document {
            get { return _document; }
        }

        #endregion Public Properties

        #region Public Constructors

        public GnosticeWordDocument(IDocument document, ILogger log = null) {
            Prevent.ParameterNull(document, nameof(document));

            _document = document;
            _log = log ?? NullLogger.Instance;
        }

        #endregion Public Constructors

        #region Private Methods

        private void OnClose() {
            Closed?.Invoke(this);
        }

        #endregion Private Methods

        #region IWordDocument Members

        public string Text {
            get {
                if (_document == null) { return null; }

                var result = string.Empty;
                try { result = _document.ToString(); } catch (Exception ex) { _log.Error(ex, $"ERROR READING DOCUMENT TEXT: {ex.Message}"); }
                return result;
            }
        }

        public void Close() {
            if (_document == null) { return; }
            try { OnClose(); } catch (Exception ex) { _log.Error(ex, $"ERROR CLOSING DOCUMENT: {ex.Message}"); }
        }

        public void Convert(string outputPath, WordConvertType type) {
            if (_document == null) { return; }
            try {
                var fileFormat = FileFormat.Auto;
                switch (type) {
                    case WordConvertType.Rtf:
                        fileFormat = FileFormat.Rtf;
                        break;

                    default:
                        fileFormat = FileFormat.Doc;
                        break;
                }
                //_document.SaveToFile(outputPath, fileFormat);
            } catch (Exception ex) { _log.Error(ex, $"ERROR CONVERTING DOCUMENT ({type}): {ex.Message}"); }
        }

        #endregion IWordDocument Members

        #region IDisposable Members

        public void Dispose() {
            /* Do Nothing */
        }

        #endregion IDisposable Members
    }
}