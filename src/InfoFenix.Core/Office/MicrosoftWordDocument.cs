using System;
using Microsoft.Office.Interop.Word;
using WordDocument = Microsoft.Office.Interop.Word.Document;

namespace InfoFenix.Core.Office {
    public class MicrosoftWordDocument : IMicrosoftWordDocument {

        #region Private Read-Only Fields

        private readonly WordDocument _document;

        #endregion Private Read-Only Fields

        #region Private Fields

        private object Unknown = Type.Missing;

        #endregion Private Fields

        #region Public Constructors

        public MicrosoftWordDocument(WordDocument document) {
            Prevent.ParameterNull(document, nameof(document));

            _document = document;
        }

        #endregion Public Constructors

        #region Private Static Methods

        private static WdSaveFormat Discovery(WordConvertType type) {
            switch (type) {
                case WordConvertType.Rtf:
                    return WdSaveFormat.wdFormatRTF;

                default:
                    return WdSaveFormat.wdFormatDocument;
            }
        }

        #endregion Private Static Methods

        #region IMicrosoftWordDocument Members

        public void Convert(string outputFilePath, WordConvertType type) {
            var currentFormat = (object)Discovery(type);
            var currentOutputFilePath = (object)outputFilePath;
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

        public string ReadContent() {
            return _document.Content.Text;
        }

        public void Close() {
            _document.Close(SaveChanges: ref Unknown
                , OriginalFormat: ref Unknown
                , RouteDocument: ref Unknown);
        }

        #endregion IMicrosoftWordDocument Members
    }
}