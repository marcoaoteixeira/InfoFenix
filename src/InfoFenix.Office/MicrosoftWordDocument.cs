using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MicrosoftDocument = Microsoft.Office.Interop.Word.Document;
using MicrosoftDocumentSaveFormat = Microsoft.Office.Interop.Word.WdSaveFormat;
using MicrosoftHeaderFooter = Microsoft.Office.Interop.Word.HeaderFooter;
using MicrosoftSection = Microsoft.Office.Interop.Word.Section;

namespace InfoFenix.Office {

    public sealed class MicrosoftWordDocument : IWordDocument, IDisposable {

        #region Special

        private object Unknown = Type.Missing;

        #endregion Special

        #region Private Fields

        private MicrosoftDocument _document;
        private bool _disposed;

        #endregion Private Fields

        #region Internal Properties

        internal MicrosoftDocument Document {
            get { return _document; }
        }

        #endregion Internal Properties

        #region Public Constructors

        public MicrosoftWordDocument(MicrosoftDocument document) {
            Prevent.ParameterNull(document, nameof(document));

            _document = document;
        }

        #endregion Public Constructors

        #region Destructor

        /// <summary>
        /// Destructor
        /// </summary>
        ~MicrosoftWordDocument() {
            Dispose(disposing: false);
        }

        #endregion Destructor

        #region Private Static Methods

        private static MicrosoftDocumentSaveFormat Discovery(DocumentFormatType type) {
            switch (type) {
                case DocumentFormatType.Rtf:
                    return MicrosoftDocumentSaveFormat.wdFormatRTF;

                default:
                    return MicrosoftDocumentSaveFormat.wdFormatDocument;
            }
        }

        #endregion Private Static Methods

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
                    Close();
                }
            }

            // Dispose your unmanaged resources here
            _document = null;
            _disposed = true;
        }

        private void ThrowIfDisposed() {
            if (_disposed) {
                throw new ObjectDisposedException(nameof(MicrosoftWordDocument));
            }
        }

        #endregion Private Methods

        #region IWordDocument Members

        public void Close() {
            ThrowIfDisposed();

            _document.Close(SaveChanges: ref Unknown
                , OriginalFormat: ref Unknown
                , RouteDocument: ref Unknown);
        }

        public Task<string> GetTextAsync(CancellationToken cancellationToken = default(CancellationToken)) {
            ThrowIfDisposed();

            return Task.Run(() => {
                var stringBuilder = new StringBuilder();
                foreach (MicrosoftSection section in _document.Sections) {
                    foreach (MicrosoftHeaderFooter header in section.Headers) {
                        stringBuilder.AppendLine(header.Range.Text);
                    }
                }
                stringBuilder.AppendLine(_document.Content.Text);
                foreach (MicrosoftSection section in _document.Sections) {
                    foreach (MicrosoftHeaderFooter footer in section.Footers) {
                        stringBuilder.AppendLine(footer.Range.Text);
                    }
                }
                return stringBuilder.ToString();
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

            var currentFormat = (object)Discovery(type);
            var currentOutputFilePath = (object)filePath;

            _document.SaveAs2(FileName: ref currentOutputFilePath,
                FileFormat: ref currentFormat,
                LockComments: ref Unknown,
                Password: ref Unknown,
                AddToRecentFiles: ref Unknown,
                WritePassword: ref Unknown,
                ReadOnlyRecommended: ref Unknown,
                EmbedTrueTypeFonts: ref Unknown,
                SaveNativePictureFormat: ref Unknown,
                SaveFormsData: ref Unknown,
                SaveAsAOCELetter: ref Unknown,
                Encoding: ref Unknown,
                InsertLineBreaks: ref Unknown,
                AllowSubstitutions: ref Unknown,
                LineEnding: ref Unknown,
                AddBiDiMarks: ref Unknown,
                CompatibilityMode: ref Unknown);
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