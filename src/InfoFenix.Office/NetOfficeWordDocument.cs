using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using NetOffice.WordApi.Enums;
using NetOfficeDocument = NetOffice.WordApi.Document;

namespace InfoFenix.Office {

    public sealed class NetOfficeWordDocument : IWordDocument, IDisposable {

        #region Private Fields

        private NetOfficeDocument _document;
        private bool _disposed;

        #endregion Private Fields

        #region Internal Properties

        internal NetOfficeDocument Document {
            get { return _document; }
        }

        #endregion Internal Properties

        #region Public Constructors

        public NetOfficeWordDocument(NetOfficeDocument document) {
            Prevent.ParameterNull(document, nameof(document));

            _document = document;
        }

        #endregion Public Constructors

        #region Destructor

        /// <summary>
        /// Destructor
        /// </summary>
        ~NetOfficeWordDocument() {
            Dispose(disposing: false);
        }

        #endregion Destructor

        #region Private Methods

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> if called from managed code; otherwise <c>false</c>.</param>
        private void Dispose(bool disposing) {
            if (_disposed) { return; }
            if (disposing) {
                // Dispose your managed resources here
                if (_document != null) {
                    _document.Close();
                    _document.Dispose();
                }
            }

            // Dispose your unmanaged resources here
            _document = null;
            _disposed = true;
        }

        private void ThrowIfDisposed() {
            if (_disposed) {
                throw new ObjectDisposedException(nameof(NetOfficeWordDocument));
            }
        }

        #endregion Private Methods

        #region IWordDocument Members

        public void Close() {
            ThrowIfDisposed();

            _document.Close();
        }

        public Task<string> GetTextAsync(CancellationToken cancellationToken = default(CancellationToken)) {
            ThrowIfDisposed();

            return Task.Run(() => {
                return _document.Content.Text;
            }, cancellationToken);
        }

        public Task<string> GetFormattedTextAsync(CancellationToken cancellationToken = default(CancellationToken)) {
            ThrowIfDisposed();

            return Task.Run(() => {
                return _document.Content.FormattedText.Text;
            }, cancellationToken);
        }

        public Task<Stream> HighlightTextAsync(CancellationToken cancellationToken = default(CancellationToken), params string[] terms) {
            ThrowIfDisposed();

            throw new NotImplementedException();
        }

        public void SaveAs(string filePath, DocumentFormatType type = DocumentFormatType.Rtf) {
            ThrowIfDisposed();

            switch (type) {
                case DocumentFormatType.Word:
                    _document.Save();
                    break;

                case DocumentFormatType.Rtf:
                    _document.SaveAs2(filePath, WdSaveFormat.wdFormatRTF);
                    break;

                default:
                    break;
            }
        }

        #endregion IWordDocument Members

        #region IDisposable Members

        /// <inheritdoc />
        public void Dispose() {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable Members
    }
}