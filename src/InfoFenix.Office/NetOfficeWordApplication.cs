using System;
using System.Collections.Generic;
using System.IO;
using NetOffice.WordApi.Enums;
using NetOfficeApplication = NetOffice.WordApi.Application;

namespace InfoFenix.Office {

    public sealed class NetOfficeWordApplication : IWordApplication, IDisposable {

        #region Private Static Read-Only Fields

        private static readonly object MissingType = Type.Missing;

        #endregion Private Static Read-Only Fields

        #region Private Constants

        private const string PROG_ID = "{BDB2B7F0-53AB-41BA-AD24-C6B3A863C873}";

        #endregion Private Constants

        #region Private Fields

        private NetOfficeApplication _application;
        private IList<NetOfficeWordDocument> _openedDocuments = new List<NetOfficeWordDocument>();
        private bool _disposed;

        #endregion Private Fields

        #region Internal Properties

        internal NetOfficeApplication Application {
            get { return _application; }
        }

        #endregion Internal Properties

        #region Public Constructors

        public NetOfficeWordApplication() {
            _application = new NetOfficeApplication() {
                Visible = false
            };
        }

        #endregion Public Constructors

        #region Destructor

        /// <summary>
        /// Destructor
        /// </summary>
        ~NetOfficeWordApplication() {
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
                    _application.Quit();
                    _application.Dispose();
                }
            }

            // Dispose your unmanaged resources here
            _application = null;
            _disposed = true;
        }

        private void ThrowIfDisposed() {
            if (_disposed) {
                throw new ObjectDisposedException(nameof(NetOfficeWordApplication));
            }
        }

        #endregion Private Methods

        #region IWordApplication Members

        public void CloseAllDocuments() {
            ThrowIfDisposed();

            foreach (var document in _openedDocuments) {
                document.Dispose();
            }
        }

        public IWordDocument Open(Stream stream) {
            ThrowIfDisposed();

            throw new NotImplementedException();
        }

        public IWordDocument Open(string filePath) {
            ThrowIfDisposed();

            try {
                var document = new NetOfficeWordDocument(_application.Documents.Open(fileName: filePath
                    , confirmConversions: MissingType
                    , readOnly: MissingType
                    , addToRecentFiles: MissingType
                    , passwordDocument: MissingType
                    , passwordTemplate: MissingType
                    , revert: MissingType
                    , writePasswordDocument: MissingType
                    , writePasswordTemplate: MissingType
                    , format: WdSaveFormat.wdFormatOpenDocumentText));

                _openedDocuments.Add(document);

                return document;
            } catch (Exception ex) { throw; }
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