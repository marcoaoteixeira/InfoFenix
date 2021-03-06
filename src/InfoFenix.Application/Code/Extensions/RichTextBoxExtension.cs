﻿using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace InfoFenix.Application.Code {

    public static class RichTextBoxExtension {

        #region Public Static Methods

        public static void Highlight(this RichTextBox source, string text, Color color = default(Color)) {
            if (source == null) { return; }
            if (string.IsNullOrWhiteSpace(text)) { return; }

            var innerColor = color == Color.Empty ? Color.Yellow : color;
            var selectionStart = source.SelectionStart;

            var innerText = text.Trim(' ', '*');
            var useRegexp = false;

            if (innerText.Contains("*")) {
                innerText = Regex.Replace(innerText, @"(\*+)", @"\S+");
                useRegexp = true;
            }

            if (useRegexp) { HighlightTextByRegexp(source, innerText, innerColor); } else { HighlightTextDefault(source, innerText, innerColor); }

            source.SelectionStart = selectionStart;
            source.SelectionLength = 0;
            source.SelectionColor = Color.Black;
        }

        #endregion Public Static Methods

        #region Private Static Methods

        private static void HighlightTextDefault(RichTextBox richTextBox, string text, Color color) {
            var index = 0;
            var startIndex = 0;
            while ((index = richTextBox.Text.IndexOf(text, startIndex, StringComparison.InvariantCultureIgnoreCase)) != -1) {
                try {
                    richTextBox.Select((index > 0 ? index - 1 : index), text.Length + 2);
                    startIndex = index + text.Length;

                    if (char.IsLetter(richTextBox.SelectedText[0]) || ((startIndex - text.Length != index) && char.IsLetter(richTextBox.SelectedText[richTextBox.SelectedText.Length - 1]))) {
                        continue; /* IGNORES IF NOT SINGLE WORD */
                    }

                    richTextBox.Select(index, text.Length);
                    richTextBox.SelectionBackColor = color;
                } catch { /* ON ERROR: IGNORE */ }
            }
        }

        private static void HighlightTextByRegexp(RichTextBox richTextBox, string text, Color color) {
            var match = Regex.Match(richTextBox.Text, text, RegexOptions.IgnoreCase);

            // First occurrence
            if (match.Success) {
                richTextBox.Select(match.Index, match.Value.Length);
                richTextBox.SelectionBackColor = color;

                // Next occurrences
                while ((match = match.NextMatch()).Success) {
                    richTextBox.Select(match.Index, match.Value.Length);
                    richTextBox.SelectionBackColor = color;
                }
            }
        }

        #endregion Private Static Methods
    }
}