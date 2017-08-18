using System;
using System.IO;
using MicrosoftApplication = Microsoft.Office.Interop.Word.Application;
using MicrosoftDocument = Microsoft.Office.Interop.Word.Document;

namespace InfoFenix.Office {

    public sealed class MicrosoftWordApplication : IWordApplication, IDisposable {

        #region Special

        private object Unknown = Type.Missing;

        #endregion Special

        #region Private Fields

        private MicrosoftApplication _application;
        private bool _disposed;

        #endregion Private Fields

        #region Internal Properties

        internal MicrosoftApplication Application {
            get { return _application; }
        }

        #endregion Internal Properties

        #region Public Constructors

        public MicrosoftWordApplication() {
            _application = new MicrosoftApplication {
                Visible = false
            };
        }

        #endregion Public Constructors

        #region Destructor

        /// <summary>
        /// Destructor
        /// </summary>
        ~MicrosoftWordApplication() {
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
                if (_application != null) {
                    _application.Quit(SaveChanges: ref Unknown
                        , OriginalFormat: ref Unknown
                        , RouteDocument: ref Unknown);
                }
            }

            // Dispose your unmanaged resources here
            _application = null;
            _disposed = true;
        }

        private void ThrowIfDisposed() {
            if (_disposed) {
                throw new ObjectDisposedException(nameof(MicrosoftWordApplication));
            }
        }

        #endregion Private Methods

        #region IWordApplication Members

        public void CloseAllDocuments() {
            ThrowIfDisposed();

            foreach (MicrosoftDocument document in _application.Documents) {
                document.Close(SaveChanges: ref Unknown
                    , OriginalFormat: ref Unknown
                    , RouteDocument: ref Unknown);
            }
        }

        public IWordDocument Open(Stream stream) {
            ThrowIfDisposed();

            throw new NotImplementedException();
        }

        public IWordDocument Open(string filePath) {
            ThrowIfDisposed();

            var currentFilePath = (object)filePath;
            var document = _application.Documents.Open(FileName: ref currentFilePath
                , ConfirmConversions: ref Unknown
                , ReadOnly: ref Unknown
                , AddToRecentFiles: ref Unknown
                , PasswordDocument: ref Unknown
                , PasswordTemplate: ref Unknown
                , Revert: ref Unknown
                , WritePasswordDocument: ref Unknown
                , WritePasswordTemplate: ref Unknown
                , Format: ref Unknown
                , Encoding: ref Unknown
                , Visible: ref Unknown
                , OpenAndRepair: ref Unknown
                , DocumentDirection: ref Unknown
                , NoEncodingDialog: ref Unknown
                , XMLTransform: ref Unknown);

            return new MicrosoftWordDocument(document);
        }

        public void Quit() {
            ThrowIfDisposed();

            Dispose(disposing: true);
        }

        #endregion IWordApplication Members

        #region IDisposable Members

        /// <inheritdoc />
        public void Dispose() {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion IDisposable Members
    }
}