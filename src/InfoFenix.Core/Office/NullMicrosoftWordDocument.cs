namespace InfoFenix.Core.Office {
    public sealed class NullMicrosoftWordDocument : IMicrosoftWordDocument {

        #region Public Static Read-Only Fields

        public static readonly IMicrosoftWordDocument Instance = new NullMicrosoftWordDocument();

        #endregion Public Static Read-Only Fields

        #region Private Constructors

        private NullMicrosoftWordDocument() {
        }

        #endregion Private Constructors

        #region IMicrosoftWordDocument Members

        public void Close() {
        }

        public void Convert(string outputFilePath, WordConvertType type) {
        }

        public string ReadContent() {
            return string.Empty;
        }

        #endregion IMicrosoftWordDocument Members
    }
}