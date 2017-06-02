using System.IO;

namespace InfoFenix.Core.Office {

    public sealed class NullWordApplication : IWordApplication {

        #region Public Static Read-Only Fields

        public static readonly IWordApplication Instance = new NullWordApplication();

        #endregion Public Static Read-Only Fields

        #region Private Constructors

        private NullWordApplication() {
        }

        #endregion Private Constructors

        #region IWordApplication Members

        public IWordDocument Open(Stream stream) {
            return NullWordDocument.Instance;
        }

        public IWordDocument Open(string filePath) {
            return NullWordDocument.Instance;
        }

        public void Quit() {
        }

        #endregion IWordApplication Members
    }
}