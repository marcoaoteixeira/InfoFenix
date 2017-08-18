using System.Collections.Generic;
using InfoFenix.Models;

namespace InfoFenix.Services {

    public interface ISearchService {

        #region Methods

        IEnumerable<SearchStatistic> GetStatistics(string[] terms, params string[] indexes);

        IEnumerable<SearchResult> ExecuteSearch(string[] terms, params string[] indexes);

        #endregion Methods
    }
}