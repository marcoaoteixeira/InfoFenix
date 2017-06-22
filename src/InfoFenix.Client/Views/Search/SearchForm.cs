using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using InfoFenix.Client.Code;
using InfoFenix.Client.Views.Shared;
using InfoFenix.Core;
using InfoFenix.Core.Cqrs;
using InfoFenix.Core.Dto;
using InfoFenix.Core.Logging;
using InfoFenix.Core.Queries;
using Resource = InfoFenix.Client.Properties.Resources;

namespace InfoFenix.Client.Views.Search {

    public partial class SearchForm : LayoutForm {

        #region Private Read-Only Fields

        private readonly IFormManager _formManager;
        private readonly IMediator _mediator;

        #endregion Private Read-Only Fields

        #region Public Properties

        private ILogger _log;
        public ILogger Log {
            get { return _log ?? NullLogger.Instance; }
            set { _log = value ?? NullLogger.Instance; }
        }

        #endregion

        #region Private Properties

        private SearchDto ViewModel { get; set; }

        #endregion Private Properties

        #region Private Fields

        private IDictionary<string, string> _currentIndexes = new Dictionary<string, string>();

        #endregion Private Fields

        #region Public Constructors

        public SearchForm(IFormManager formManager, IMediator mediator) {
            Prevent.ParameterNull(formManager, nameof(formManager));
            Prevent.ParameterNull(mediator, nameof(mediator));

            _formManager = formManager;
            _mediator = mediator;

            InitializeComponent();
        }

        #endregion Public Constructors

        #region Private Methods

        private void Initialize() {
            documentDirectoriesDataGridView.AutoGenerateColumns = false;

            _currentIndexes = _mediator
                .Query(new ListDocumentDirectoriesQuery())
                .ToDictionary(_ => _.Code, _ => _.Label);

            ViewModel = _mediator.Query(new SearchDocumentIndexQuery {
                QueryTerm = string.Empty,
                Indexes = _currentIndexes
            });

            documentDirectoriesDataGridView.DataSource = ViewModel.Indexes.ToArray();
        }

        private void DoSearch() {
            ViewModel = WaitFor.Instance.Execution(() => {
                return _mediator.Query(new SearchDocumentIndexQuery {
                    QueryTerm = searchTermTextBox.Text,
                    Indexes = _currentIndexes
                });
            }, log: Log, silent: true);

            documentDirectoriesDataGridView.DataSource = ViewModel.Indexes.ToArray();

            informationLabel.Text = string.Empty;
            if (ViewModel.Indexes.SelectMany(_ => _.Documents).Count() == 0) {
                informationLabel.Text = Resource.SearchForm_EmptyResultSet;
            }
        }

        private void ShowSearchResult(IndexDto index) {
            if (index.Documents.Count == 0) { return; }

            var form = _formManager.Get<SearchResultForm>(mdi: MdiParent, multipleInstance: true);
            form.SearchTerms = ViewModel.QueryTerm.Split(' ');
            form.SearchResult = index;
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
                ShowSearchResult(dataGridView.Rows[e.RowIndex].DataBoundItem as IndexDto);
            }
        }

        private void closeButton_Click(object sender, EventArgs e) {
            Close();
        }

        #endregion Event Handlers
    }
}