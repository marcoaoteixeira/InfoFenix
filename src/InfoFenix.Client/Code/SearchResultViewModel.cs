using System.Collections.Generic;

namespace InfoFenix.Client.Code {

    public class SearchResultViewModel {

        #region Public Properties

        public string SearchTerm { get; set; }

        public IList<DocumentDirectoryViewModel> Items { get; set; } = new List<DocumentDirectoryViewModel>();

        #endregion Public Properties
    }
}