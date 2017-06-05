using System.Collections.Generic;

namespace InfoFenix.Client.Code {

    public class SearchResultViewModel {

        #region Public Properties

        public string SearchTerm { get; set; }

        public IList<SearchResultItemViewModel> Items { get; set; } = new List<SearchResultItemViewModel>();

        #endregion Public Properties
    }
}