using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace InfoFenix.Core.Office {

    public enum DocumentFormatType {
        Word,
        Rtf
    }

    public interface IWordDocument : IDisposable {

        #region Methods

        Task<string> GetTextAsync(CancellationToken cancellationToken = default(CancellationToken));

        Task<Stream> HighlightTextAsync(CancellationToken cancellationToken = default(CancellationToken), params string[] terms);

        void SaveAs(string filePath, DocumentFormatType type = DocumentFormatType.Rtf);

        void Close();

        #endregion Methods
    }
}