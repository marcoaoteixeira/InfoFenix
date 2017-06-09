using System;
using System.Linq;
using System.Windows.Forms;
using InfoFenix.Client.Code;
using InfoFenix.Client.Models;
using InfoFenix.Client.Views.Shared;
using InfoFenix.Core;
using InfoFenix.Core.Services;

namespace InfoFenix.Client.Views {

    public partial class SearchForm : LayoutForm {

        #region Private Read-Only Fields

        private readonly IDocumentDirectoryService _documentDirectoryService;
        private readonly IFormManager _formManager;
        private readonly ISearchIndexService _searchIndexService;

        #endregion Private Read-Only Fields

        #region Private Properties

        private SearchViewModel ViewModel { get; set; }

        #endregion Private Properties

        #region Public Constructors

        public SearchForm(IDocumentDirectoryService documentDirectoryService, IFormManager formManager, ISearchIndexService searchIndexService) {
            Prevent.ParameterNull(documentDirectoryService, nameof(documentDirectoryService));
            Prevent.ParameterNull(formManager, nameof(formManager));
            Prevent.ParameterNull(searchIndexService, nameof(searchIndexService));

            _documentDirectoryService = documentDirectoryService;
            _formManager = formManager;
            _searchIndexService = searchIndexService;

            InitializeComponent();
        }

        #endregion Public Constructors

        #region Private Methods

        private void Initialize() {
            ViewModel = new SearchViewModel();
            searchResultDataGridView.AutoGenerateColumns = false;
            foreach (var documentDirectory in _documentDirectoryService.List()) {
                ViewModel.Items.Add(new SearchDocumentDirectoryViewModel {
                    Code = documentDirectory.Code,
                    Label = documentDirectory.Label,
                    TotalDocuments = documentDirectory.TotalDocuments
                });
            }
            searchResultDataGridView.DataSource = ViewModel.Items.ToArray();
        }

        private void DoSearch() {
            try {
                Cursor = Cursors.WaitCursor;
                ViewModel.SearchTerm = searchTermTextBox.Text.Trim();

                var result = _searchIndexService.Search(ViewModel.SearchTerm, ViewModel.Items.Select(_ => _.Code).ToArray());
                foreach (var set in result) {
                    var documentDirectory = ViewModel.Items.SingleOrDefault(_ => _.Code == set.IndexName);
                    if (documentDirectory == null) { continue; }

                    documentDirectory.Documents.Clear();
                    foreach (dynamic item in set) {
                        documentDirectory.Documents.Add(new SearchDocumentViewModel {
                            ID = item.ID,
                            Code = item.Code,
                            Path = item.Path,
                            Payload = item.Payload
                        });
                    }
                }
                searchResultDataGridView.DataSource = ViewModel.Items.ToArray();

                informationLabel.Text = string.Empty;
                if (ViewModel.Items.SelectMany(_ => _.Documents).Count() == 0) {
                    informationLabel.Text = "A pesquisa não retornou resultados.";
                }
            } finally { Cursor = Cursors.Default; }
        }

        private void ShowSearchResult(SearchDocumentDirectoryViewModel searchDocumentDirectory) {
            if (searchDocumentDirectory.TotalDocumentsFound == 0) { return; }

            var form = _formManager.Get<SearchResultForm>(mdi: MdiParent, multipleInstance: true);
            form.SearchTerms = ViewModel.SearchTerm.Split(' ');
            form.SearchResult = searchDocumentDirectory;
            form.Show();
        }

        #endregion Private Methods

        #region Event Handlers

        private void SearchForm_Load(object sender, EventArgs e) {
            Initialize();
        }

        private void searchTermTextBox_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter && !string.IsNullOrWhiteSpace(searchTermTextBox.Text)) {
                DoSearch();
            }
        }

        private void executeSearchButton_Click(object sender, EventArgs e) {
            if (!(sender is Button button)) { return; }
            if (string.IsNullOrWhiteSpace(searchTermTextBox.Text)) { return; }

            DoSearch();
        }

        private void searchResultDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e) {
            if (sender is DataGridView view) {
                ShowSearchResult(view.Rows[e.RowIndex].DataBoundItem as SearchDocumentDirectoryViewModel);
            }
        }

        #endregion Event Handlers
    }
}