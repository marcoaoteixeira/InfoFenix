using System;
using System.IO;
using System.Windows.Forms;
using InfoFenix.Client.Code;
using InfoFenix.Client.Views.Shared;
using InfoFenix.Core;
using InfoFenix.Core.CQRS;
using InfoFenix.Core.Entities;
using InfoFenix.Core.Queries;
using Resource = InfoFenix.Client.Properties.Resources;

namespace InfoFenix.Client.Views.Search {

    public partial class SearchResultForm : LayoutForm {

        #region Private Read-Only Fields

        private readonly string _tempFilePath = Path.Combine(Path.GetTempPath(), Path.GetTempFileName());
        private readonly IMediator _mediator;

        #endregion Private Read-Only Fields

        #region Private Fields

        private int _position;

        #endregion Private Fields

        #region Public Properties

        public string[] SearchTerms { get; set; }

        public IndexDto SearchResult { get; set; }

        #endregion Public Properties

        #region Public Constructors

        public SearchResultForm(IMediator mediator) {
            Prevent.ParameterNull(mediator, nameof(mediator));

            _mediator = mediator;

            InitializeComponent();
        }

        #endregion Public Constructors

        #region Private Methods

        private void Initialize() {
            informationLabel.Text = string.Empty;

            ShowDocument(0); // Show first document.
        }

        private void ShowDocument(int index) {
            if (SearchResult == null) { return; }

            var documentIndex = SearchResult.DocumentsFound[index];
            var document = _mediator.Query(new GetDocumentQuery {
                ID = documentIndex.DocumentID,
                FetchPayload = false
            });

            documentViewerRichTextBox.Text = document.Content;

            HighlightSearchTerms(SearchTerms);

            informationLabel.Text = string.Format(Resource.SearchResultForm_InformationLabel, index + 1, SearchResult.DocumentsFound.Count);
            documentPathLabel.Text = string.Format(Resource.SearchResultForm_DocumentPathLabel, documentIndex.FileName);
        }

        private void HighlightSearchTerms(params string[] terms) {
            foreach (var term in terms) {
                if (term.StartsWith("-")) { continue; }

                documentViewerRichTextBox.Highlight(term);
            }
        }

        private void CopyTextToClipboard(string text) {
            if (string.IsNullOrWhiteSpace(text)) { return; }

            Clipboard.SetText(text);

            MessageBox.Show(Resource.TextCopiedToClipboard
                , Resource.Copy
                , MessageBoxButtons.OK
                , MessageBoxIcon.Information);
        }

        #endregion Private Methods

        #region Event Handlers

        private void SearchResultForm_Load(object sender, EventArgs e) {
            Initialize();
        }

        private void SearchResultForm_FormClosing(object sender, FormClosingEventArgs e) {
            if (File.Exists(_tempFilePath)) { File.Delete(_tempFilePath); }
        }

        private void firstResultButton_Click(object sender, EventArgs e) {
            var button = sender as Button;
            if (button == null) { return; }

            _position = 0;
            ShowDocument(_position);
        }

        private void previousResultButton_Click(object sender, EventArgs e) {
            var button = sender as Button;
            if (button == null) { return; }

            if (_position > 0) {
                _position--;
            }
            ShowDocument(_position);
        }

        private void nextResultButton_Click(object sender, EventArgs e) {
            if (SearchResult == null) { return; }

            var button = sender as Button;
            if (button == null) { return; }

            if (_position < SearchResult.DocumentsFound.Count - 1) {
                _position++;
            }
            ShowDocument(_position);
        }

        private void lastResultButton_Click(object sender, EventArgs e) {
            if (SearchResult == null) { return; }

            var button = sender as Button;
            if (button == null) { return; }

            _position = SearchResult.DocumentsFound.Count - 1;
            ShowDocument(_position);
        }

        private void closeButton_Click(object sender, EventArgs e) {
            Close();
        }

        private void copyTextButton_Click(object sender, EventArgs e) {
            var button = sender as Button;
            if (button == null) { return; }

            if (string.IsNullOrWhiteSpace(documentViewerRichTextBox.Text)) { return; }

            CopyTextToClipboard(documentViewerRichTextBox.Text);
        }

        private void copyTextToolStripMenuItem_Click(object sender, EventArgs e) {
            CopyTextToClipboard(documentViewerRichTextBox.SelectedText);
        }

        #endregion Event Handlers
    }
}