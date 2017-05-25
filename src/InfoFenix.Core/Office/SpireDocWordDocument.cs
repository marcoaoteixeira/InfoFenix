using System;
using System.IO;
using InfoFenix.Core.Logging;
using Spire.Doc;

namespace InfoFenix.Core.Office {

    public sealed class SpireDocWordDocument : IWordDocument, IDisposable {

        #region Private Read-Only Fields

        private readonly Stream _stream;
        private readonly ILogger _log;

        #endregion Private Read-Only Fields

        #region Private Fields

        private Document _document;
        private bool _disposed;

        #endregion Private Fields

        #region Public Constructors

        public SpireDocWordDocument(Stream stream, ILogger log = null) {
            Prevent.ParameterNull(stream, nameof(stream));

            _stream = stream;
            _log = log ?? NullLogger.Instance;

            Initialize();
        }

        #endregion Public Constructors

        #region Destructor

        ~SpireDocWordDocument() {
            Dispose(disposing: false);
        }

        #endregion Destructor

        #region Private Methods

        private void Initialize() {
            try { _document = new Document(_stream); }
            catch (Exception ex) { _log.Error(ex, $"ERROR OPENING DOCUMENT: {ex.Message}"); throw; }
        }

        private void Dispose(bool disposing) {
            if (_disposed) { return; }
            if (disposing) { Close(); }
            _document = null;
            _disposed = true;
        }

        #endregion Private Methods

        #region IWordDocument Members

        public string Text {
            get {
                if (_document == null) { return null; }

                var result = string.Empty;
                try { result = _document.GetText(); }
                catch (Exception ex) { _log.Error(ex, $"ERROR READING DOCUMENT TEXT: {ex.Message}"); }
                return result;
            }
        }

        public void Close() {
            if (_document == null) { return; }
            try {
                _document.Close();
                _document.Dispose();
                _document = null;
            }
            catch (Exception ex) { _log.Error(ex, $"ERROR CLOSING DOCUMENT: {ex.Message}"); }
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
                _document.SaveToFile(outputPath, fileFormat);
            } catch (Exception ex) { _log.Error(ex, $"ERROR CONVERTING DOCUMENT ({type}): {ex.Message}"); }
        }

        #endregion IWordDocument Members

        #region IDisposable Members

        public void Dispose() {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable Members
    }
}