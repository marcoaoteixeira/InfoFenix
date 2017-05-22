using System.Globalization;
using System.Net;
using System.Text;

namespace InfoFenix.Core {
    public static class StringExtension {

        #region Public Static Methods

        /// <summary>
        /// Remove diacritics from <paramref name="source"/> <see cref="string"/>.
        /// Diacritics are signs, such as an accent or cedilla, which when written above or below a letter indicates
        /// a difference in pronunciation from the same letter when unmarked or differently marked.
        /// </summary>
        /// <param name="source">The source <see cref="string"/>.</param>
        /// <returns>A new <see cref="string"/> without diacritics.</returns>
        public static string RemoveDiacritics(this string source) {
            if (string.IsNullOrWhiteSpace(source)) {
                return source;
            }

            var normalized = source.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var @char in normalized) {
                if (CharUnicodeInfo.GetUnicodeCategory(@char) != UnicodeCategory.NonSpacingMark) {
                    stringBuilder.Append(@char);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        public static string RemoveHtmlTags(this string source, bool htmlDecode = false) {
            if (string.IsNullOrEmpty(source)) {
                return string.Empty;
            }

            var content = new char[source.Length];

            var cursor = 0;
            var inside = false;
            for (var idx = 0; idx < source.Length; idx++) {
                char current = source[idx];

                switch (current) {
                    case '<':
                        inside = true;
                        continue;
                    case '>':
                        inside = false;
                        continue;
                }

                if (!inside) {
                    content[cursor++] = current;
                }
            }

            var result = new string(content, 0, cursor);
            if (htmlDecode) {
                result = WebUtility.HtmlDecode(result);
            }

            return result;
        }

        public static byte[] ToByteArray(this string source) {
            if (source == null) { return null; }

            return Encoding.UTF8.GetBytes(source);
        }

        #endregion Public Static Methods
    }
}