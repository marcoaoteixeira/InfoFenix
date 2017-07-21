using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace InfoFenix.Core.Office {

    public sealed class NullWordDocument : IWordDocument {

        #region Public Static Read-Only Fields

        public static readonly IWordDocument Instance = new NullWordDocument();

        #endregion Public Static Read-Only Fields

        #region Private Constructors

        private NullWordDocument() {
        }

        #endregion Private Constructors

        #region IWordDocument Members

        public Task<string> GetTextAsync(CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.FromResult(string.Empty);
        }

        public void Close() {
        }

        public Task<Stream> HighlightTextAsync(CancellationToken cancellationToken = default(CancellationToken), params string[] terms) {
            return Task.FromResult(Stream.Null);
        }

        public void SaveAs(string filePath, DocumentFormatType type = DocumentFormatType.Rtf) {

        }

        #endregion IWordDocument Members

        #region IDisposable Members

        public void Dispose() {
        }

        #endregion IDisposable Members
    }
}