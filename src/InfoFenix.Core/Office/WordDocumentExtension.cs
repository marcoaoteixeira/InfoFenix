using System.IO;
using System.Threading;

namespace InfoFenix.Office {

    public static class WordDocumentExtension {

        #region Public Static Methods

        public static string GetText(this IWordDocument source) {
            if (source == null) { return null; }

            return source.GetTextAsync(CancellationToken.None).WaitForResult();
        }

        public static Stream HighlightText(this IWordDocument source, params string[] terms) {
            if (source == null) { return null; }

            return source.HighlightTextAsync(CancellationToken.None, terms).WaitForResult();
        }

        #endregion Public Static Methods
    }
}