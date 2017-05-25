using System.IO;

namespace InfoFenix.Core.Office {

    public sealed class NullWordDocumentService : IWordDocumentService {

        #region Public Static Read-Only Fields

        public static readonly IWordDocumentService Instance = new NullWordDocumentService();

        #endregion Public Static Read-Only Fields

        #region Private Constructors

        private NullWordDocumentService() {
        }

        #endregion Private Constructors

        #region IWordDocumentService Members

        public IWordDocument Open(Stream stream) {
            return NullWordDocument.Instance;
        }

        public IWordDocument Open(string filePath) {
            return NullWordDocument.Instance;
        }

        #endregion IWordDocumentService Members
    }
}