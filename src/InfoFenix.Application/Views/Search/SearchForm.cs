using System;
using System.Linq;
using System.Windows.Forms;
using InfoFenix.Application.Code;
using InfoFenix.Application.Models;
using InfoFenix.Application.Views.Shared;
using InfoFenix;
using InfoFenix.CQRS;
using InfoFenix.Logging;
using InfoFenix.Domains.Queries;
using InfoFenix.Services;
using InfoFenix.Resources;

namespace InfoFenix.Application.Views.Search {

    public partial class SearchForm : LayoutForm {

        #region Private Read-Only Fields

        private readonly IFormManager _formManager;
        private readonly IMediator _mediator;
        private readonly ISearchService _searchService;

        #endregion Private Read-Only Fields

        #region Public Properties

        private ILogger _log;

        public ILogger Log {
            get { return _log ?? NullLogger.Instance; }
            set { _log = value ?? NullLogger.Instance; }
        }

        #endregion Public Properties

        #region Private Properties

        private SearchIndexViewModel[] SearchIndexes { get; set; }

        #endregion Private Properties

        #region Public Constructors

        public SearchForm(IFormManager formManager, IMediator mediator, ISearchService searchService) {
            Prevent.ParameterNull(formManager, nameof(formManager));
            Prevent.ParameterNull(mediator, nameof(mediator));
            Prevent.ParameterNull(searchService, nameof(searchService));

            _formManager = formManager;
            _mediator = mediator;
            _searchService = searchService;

            InitializeComponent();
        }

        #endregion Public Constructors

        #region Private Methods

        private void Initialize() {
            documentDirectoriesDataGridView.AutoGenerateColumns = false;
            SearchIndexes = _mediator
                .Query(new ListDocumentDirectoriesQuery())
                .OrderBy(_ => _.Position)
                .Select(_ => new SearchIndexViewModel {
                    IndexLabel = _.Label,
                    IndexName = _.Code,
                    TotalDocuments = _.TotalDocuments,
                    TotalDocumentsFound = 0
                })
                .ToArray();
            documentDirectoriesDataGridView.DataSource = SearchIndexes.ToArray();
        }

        private void DoSearch() {
            var statistics = WaitFor.Execution(() => {
                return _searchService.GetStatistics(
                    terms: searchTermTextBox.Text.Split(' '),
                    indexes: SearchIndexes.Select(_ => _.IndexName).ToArray()
                );
            }, log: Log, silent: true);

            SearchIndexes.Each(index => {
                var stats = statistics.SingleOrDefault(statistic => statistic.IndexName == index.IndexName);
                if (stats != null) {
                    index.TotalDocumentsFound = stats.TotalDocumentsFound;
                }
            });

            documentDirectoriesDataGridView.DataSource = SearchIndexes.ToArray();

            informationLabel.Text = statistics.All(_ => _.TotalDocumentsFound == 0)
                ? Strings.SearchForm_EmptyResultSet
                : string.Empty;
        }

        private void ShowSearchResult(SearchIndexViewModel index) {
            if (index.TotalDocumentsFound == 0) { return; }

            var form = _formManager.Get<SearchResultForm>(mdi: MdiParent, multipleInstance: true);
            var searchResult = _searchService.ExecuteSearch(
                terms: searchTermTextBox.Text.Split(' '),
                indexes: index.IndexName
            ).SingleOrDefault();
            form.SearchResult = searchResult;
            form.Show();
        }

        #endregion Private Methods

        #region Event Handlers

        private void SearchForm_Load(object sender, EventArgs e) {
            Initialize();
        }

        private void searchTermTextBox_KeyDown(object sender, KeyEventArgs e) {
            if (e.KeyCode == Keys.Enter && !string.IsNullOrWhiteSpace(searchTermTextBox.Text) && searchTermTextBox.Text.Trim().Length > 1) {
                DoSearch();
            }
        }

        private void executeSearchButton_Click(object sender, EventArgs e) {
            if (!(sender is Button button)) { return; }
            if (string.IsNullOrWhiteSpace(searchTermTextBox.Text) && searchTermTextBox.Text.Trim().Length < 2) { return; }

            DoSearch();
        }

        private void searchResultDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e) {
            if (sender is DataGridView dataGridView) {
                ShowSearchResult(dataGridView.Rows[e.RowIndex].DataBoundItem as SearchIndexViewModel);
            }
        }

        private void closeButton_Click(object sender, EventArgs e) {
            Close();
        }

        #endregion Event Handlers
    }
}