using System.Collections.Generic;

namespace InfoFenix.Client.Models {

    public class SearchDocumentDirectoryViewModel {

        #region Public Properties

        public string Code { get; set; }

        public string Label { get; set; }

        public int TotalDocuments { get; set; }

        public int TotalDocumentsFound {
            get { return Documents != null ? Documents.Count : 0; }
        }

        public IList<SearchDocumentViewModel> Documents { get; set; } = new List<SearchDocumentViewModel>();

        #endregion Public Properties
    }
}