using System;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
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

        private string _path;

        #endregion Private Fields

        #region Public Events

        public event Action<SpireDocWordDocument> Closed;

        #endregion Public Events

        #region Public Constructors

        public SpireDocWordDocument(string path, Stream stream, ILogger log = null) {
            Prevent.ParameterNullOrWhiteSpace(path, nameof(path));
            Prevent.ParameterNull(stream, nameof(stream));

            _path = path;
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
            catch (Exception) { _log.Error($"ERROR OPENING DOCUMENT: {_path}"); throw; }
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

        public Task<string> GetTextAsync(CancellationToken cancellationToken = default(CancellationToken)) {
            if (_document == null) { return Task.FromResult((string)null); }

            return Task.Run(() => {
                var result = string.Empty;
                try { result = _document.GetText(); }
                catch (Exception ex) { _log.Error(ex, $"ERROR READING DOCUMENT TEXT: {_path}"); }
                return result;
            }, cancellationToken);
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
            } catch (Exception ex) { _log.Error(ex, $"ERROR CLOSING DOCUMENT: {_path}"); }
        }

        public Task<Stream> HighlightTextAsync(CancellationToken cancellationToken = default(CancellationToken), params string[] terms) {
            if (terms == null) { return Task.FromResult(Stream.Null); }

            return Task.Run(() => {
                foreach (var term in terms) {
                    if (cancellationToken.IsCancellationRequested) { break; }

                    var currentTerm = (term.IndexOf('*') > 0) && (term.IndexOf('*') < term.Length - 1)
                        ? term.Replace("*", ".*")
                        : term;

                    var occurences = currentTerm.Contains(".*")
                        ? _document.FindAllPattern(new Regex(currentTerm))
                        : _document.FindAllString(term, caseSensitive: false, wholeWord: false);

                    if (occurences == null) { continue; }
                    foreach (var occurrence in occurences) {
                        if (cancellationToken.IsCancellationRequested) { break; }

                        occurrence.GetAsOneRange().CharacterFormat.HighlightColor = Color.Yellow;
                    }
                }

                Stream result = new MemoryStream();
                _document.SaveToStream(result, _document.DetectedFormatType);
                return result;
            }, cancellationToken);
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