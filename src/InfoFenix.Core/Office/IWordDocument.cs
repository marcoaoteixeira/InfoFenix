using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace InfoFenix.Office {

    public enum DocumentFormatType {
        Word,
        Rtf
    }

    public interface IWordDocument {

        #region Methods

        Task<string> GetTextAsync(CancellationToken cancellationToken = default(CancellationToken));

        Task<string> GetFormattedTextAsync(CancellationToken cancellationToken = default(CancellationToken));

        Task<Stream> HighlightTextAsync(CancellationToken cancellationToken = default(CancellationToken), params string[] terms);

        void SaveAs(string filePath, DocumentFormatType type = DocumentFormatType.Rtf);

        void Close();

        #endregion Methods
    }
}