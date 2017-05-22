namespace InfoFenix.Core.Office {

    public sealed class NullMicrosoftWordApplication : IMicrosoftWordApplication {

        #region Public Static Read-Only Fields

        public static readonly IMicrosoftWordApplication Instance = new NullMicrosoftWordApplication();

        #endregion Public Static Read-Only Fields

        #region Private Constructors

        private NullMicrosoftWordApplication() {
        }

        #endregion Private Constructors

        #region IMicrosoftWordApplication Members

        public IMicrosoftWordDocument Open(string filePath) {
            return NullMicrosoftWordDocument.Instance;
        }

        public void Quit() { }

        #endregion IMicrosoftWordApplication Members
    }
}