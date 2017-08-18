namespace InfoFenix.Application.Models {

    public sealed class SearchIndexViewModel {

        #region Public Properties

        public string IndexLabel { get; set; }

        public string IndexName { get; set; }

        public int TotalDocuments { get; set; }

        public int TotalDocumentsFound { get; set; }

        #endregion Public Properties
    }
}