using System;
using System.Drawing;
using System.IO;
using InfoFenix.Core.Logging;
using Spire.Doc;

namespace InfoFenix.Core.Office {

    public sealed class SpireDocWordDocument : IWordDocument, IDisposable {

        #region Private Read-Only Fields

        private readonly ILogger _log;

        #endregion Private Read-Only Fields

        #region Private Fields

        private bool _disposed;
        private Document _document;
        private Stream _stream;

        #endregion Private Fields

        #region Public Events

        public event Action<SpireDocWordDocument> Closed;

        #endregion Public Events

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
            try { _document = new Document(_stream); } catch (Exception ex) { _log.Error(ex, $"ERROR OPENING DOCUMENT: {ex.Message}"); throw; }
        }

        private void Dispose(bool disposing) {
            if (_disposed) { return; }
            if (disposing) { Close(); }
            _document = null;
            _stream = null;
            _disposed = true;
        }

        private void OnClose() {
            Closed?.Invoke(this);
        }

        #endregion Private Methods

        #region IWordDocument Members

        public string Text {
            get {
                if (_document == null) { return null; }

                var result = string.Empty;
                try { result = _document.GetText(); } catch (Exception ex) { _log.Error(ex, $"ERROR READING DOCUMENT TEXT: {ex.Message}"); }
                return result;
            }
        }

        public void Close() {
            try {
                if (_document != null) {
                    _document.Close();
                    _document.Dispose();
                    _document = null;
                }

                if (_stream != null) {
                    _stream.Dispose();
                    _stream = null;
                }

                OnClose();
            } catch (Exception ex) { _log.Error(ex, $"ERROR CLOSING DOCUMENT: {ex.Message}"); }
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

        public void Convert(Stream outputStream, WordConvertType type) {
            Prevent.ParameterNull(outputStream, nameof(outputStream));

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
                _document.SaveToStream(outputStream, fileFormat);
            } catch (Exception ex) { _log.Error(ex, $"ERROR CONVERTING DOCUMENT ({type}): {ex.Message}"); }
        }

        public void Highlight(Stream outputStream, params string[] terms) {
            if (terms == null) { return; }

            foreach (var term in terms) {
                var occurences = _document.FindAllString(term, false, false);
                if (occurences == null) { continue; }
                foreach (var occurrence in occurences) {
                    occurrence.GetAsOneRange().CharacterFormat.HighlightColor = Color.Yellow;
                }
            }
            _document.SaveToStream(outputStream, _document.DetectedFormatType);
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