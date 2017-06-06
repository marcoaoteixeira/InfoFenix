using System.Collections.Generic;
using System.Linq;

namespace InfoFenix.Client.Code {

    public class DocumentDirectoryViewModel {

        #region Public Properties

        public string Code { get; set; }

        public string Label { get; set; }

        public int TotalDocuments { get; set; }

        public int TotalDocumentsFound {
            get { return DocumentsFound != null ? DocumentsFound.Count() : 0; }
        }

        public IList<DocumentViewModel> DocumentsFound { get; set; } = new List<DocumentViewModel>();

        #endregion Public Properties
    }
}