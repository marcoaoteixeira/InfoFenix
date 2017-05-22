using System;
using System.Drawing;
using System.Windows.Forms;

namespace InfoFenix.Client.Code {

    public static class RichTextBoxExtension {

        #region Public Static Methods

        public static void Highlight(this RichTextBox source, string text, Color color) {
            if (source == null) { return; }
            if (string.IsNullOrWhiteSpace(text)) { return; }

            var selectionStart = source.SelectionStart;
            var startIndex = 0;
            var index = 0;

            while ((index = source.Text.IndexOf(text, startIndex, StringComparison.InvariantCultureIgnoreCase)) != -1) {
                source.Select(index, text.Length);
                source.SelectionBackColor = color;

                startIndex = index + text.Length;
            }

            source.SelectionStart = selectionStart;
            source.SelectionLength = 0;
            source.SelectionColor = Color.Black;
        }

        #endregion Public Static Methods
    }
}