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

        public string Text => string.Empty;

        public void Close() {
        }

        public void Convert(string outputPath, WordConvertType type) {
        }

        #endregion IWordDocument Members

        #region IDisposable Members

        public void Dispose() { }

        #endregion
    }
}