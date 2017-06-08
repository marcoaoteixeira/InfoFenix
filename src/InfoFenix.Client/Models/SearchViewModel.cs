using System.Collections.Generic;

namespace InfoFenix.Client.Models {

    public class SearchViewModel {

        #region Public Properties

        public string SearchTerm { get; set; }

        public IList<SearchDocumentDirectoryViewModel> Items { get; set; } = new List<SearchDocumentDirectoryViewModel>();

        #endregion Public Properties
    }
}