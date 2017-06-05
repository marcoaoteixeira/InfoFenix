using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace InfoFenix.Core.Services {

    public class SearchIndexResultSet : HashSet<SearchIndexCollection> {

        #region Public Static Read-Only Fields

        public static readonly SearchIndexResultSet Empty = new SearchIndexResultSet();

        #endregion Public Static Read-Only Fields

        #region Public Properties

        public SearchIndexCollection this[string indexName] {
            get { return this.SingleOrDefault(_ => string.Equals(_.IndexName, indexName, StringComparison.CurrentCultureIgnoreCase)); }
        }

        #endregion Public Properties
    }

    public class SearchIndexCollection : Collection<SearchIndexItem> {

        #region Public Properties

        public string IndexName { get; set; }

        public int TotalDocuments { get; set; }

        #endregion Public Properties

        #region Public Methods

        public bool Equals(SearchIndexCollection obj) {
            return obj != null && string.Equals(obj.IndexName, IndexName, StringComparison.CurrentCultureIgnoreCase);
        }

        #endregion Public Methods

        #region Public Override Methods

        public override bool Equals(object obj) {
            return Equals(obj as SearchIndexCollection);
        }

        public override int GetHashCode() {
            return IndexName != null ? IndexName.GetHashCode() : 0;
        }

        #endregion Public Override Methods
    }

    public class SearchIndexItem : Expando { }

    public interface ISearchIndexService {

        #region Methods

        SearchIndexResultSet Search(string term, params string[] indexNames);

        #endregion Methods
    }
}