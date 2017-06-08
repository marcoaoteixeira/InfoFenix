using System;
using System.Linq;
using System.Windows.Forms;
using InfoFenix.Client.Models;
using InfoFenix.Client.Views.Shared;
using InfoFenix.Core;
using InfoFenix.Core.Services;

namespace InfoFenix.Client.Views {

    public partial class SearchForm : LayoutForm {

        #region Private Read-Only Fields

        private readonly IDocumentDirectoryService _documentDirectoryService;
        private readonly ISearchIndexService _searchService;

        #endregion Private Read-Only Fields

        #region Private Properties

        private SearchViewModel ViewModel { get; set; }

        #endregion Private Properties

        #region Public Constructors

        public SearchForm(IDocumentDirectoryService documentDirectoryService, ISearchIndexService searchService) {
            Prevent.ParameterNull(documentDirectoryService, nameof(documentDirectoryService));
            Prevent.ParameterNull(searchService, nameof(searchService));

            _documentDirectoryService = documentDirectoryService;
            _searchService = searchService;

            InitializeComponent();
        }

        #endregion Public Constructors

        #region Private Methods

        private void Initialize() {
            searchResultDataGridView.AutoGenerateColumns = false;
        }

        private void DoSearch() {
            ViewModel.SearchTerm = searchTermTextBox.Text.Trim();

            // Clean previous result
            ViewModel.Items.Clear();

            var result = _searchService.Search(ViewModel.SearchTerm);
            foreach (var set in result) {
                var documentDirectory = _documentDirectoryService.List(code: set.IndexName).SingleOrDefault();
                var list = new SearchDocumentDirectoryViewModel {
                    Code = set.IndexName,
                    Label = documentDirectory.Label,
                    TotalDocuments = set.TotalDocuments
                };
                foreach (dynamic item in set) {
                    list.Documents.Add(new SearchDocumentViewModel {
                        ID = item.ID,
                        Code = item.Code,
                        Path = item.Path,
                        Payload = item.Payload
                    });
                }
                ViewModel.Items.Add(list);
            }

            // Update grid view
        }

        #endregion Private Methods

        #region Event Handlers

        private void SearchForm_Load(object sender, EventArgs e) {
            Initialize();
        }

        private void executeSearchButton_Click(object sender, EventArgs e) {
            if (!(sender is Button button)) { return; }
            if (string.IsNullOrWhiteSpace(searchTermTextBox.Text)) { return; }

            DoSearch();
        }

        private void searchResultDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e) {
        }

        #endregion Event Handlers
    }
}