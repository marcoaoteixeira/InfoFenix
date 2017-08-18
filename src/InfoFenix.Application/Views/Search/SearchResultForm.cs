using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using InfoFenix.Application.Code;
using InfoFenix.Application.Views.Shared;
using InfoFenix;
using InfoFenix.CQRS;
using InfoFenix.Dto;
using InfoFenix.Models;
using InfoFenix.Domains.Queries;
using InfoFenix.Resources;

namespace InfoFenix.Application.Views.Search {

    public partial class SearchResultForm : LayoutForm {

        #region Private Read-Only Fields

        private readonly string _tempFilePath = Path.Combine(Path.GetTempPath(), Path.GetTempFileName());

        private readonly IMediator _mediator;

        #endregion Private Read-Only Fields

        #region Private Fields

        private Dto.DocumentIndexDto _current;
        private int _position;
        private bool _useImprovedViewOnDocumentViewer;

        private Font _documentViewerTextFont;
        private Color _documentViewerTextColor;

        #endregion Private Fields

        #region Public Properties

        public SearchResult SearchResult { get; set; }

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

            _documentViewerTextColor = documentViewerRichTextBox.ForeColor;
            _documentViewerTextFont = (Font)documentViewerRichTextBox.Font.Clone();

            ShowDocument(0); // Show first document.
        }

        private void ShowDocument(int index) {
            if (SearchResult == null) { return; }
            if (_current != null && _current.DocumentID == SearchResult.Documents[index].DocumentID) { return; }

            _current = SearchResult.Documents[index];

            ShowDocumentInDocumentViewer();

            HighlightSearchTerms(SearchResult.Terms);

            informationLabel.Text = string.Format(Strings.SearchResultForm_InformationLabel, index + 1, SearchResult.Documents.Length);
            documentPathLabel.Text = string.Format(Strings.SearchResultForm_DocumentPathLabel, _current.FileName);
        }

        private void ShowDocumentInDocumentViewer() {
            if (_current == null) { return; }
            WaitFor.Execution(() => {
                if (_useImprovedViewOnDocumentViewer) {
                    var document = _mediator.Query(new GetDocumentQuery {
                        FetchPayload = true,
                        ID = _current.DocumentID
                    });
                    File.WriteAllBytes(_tempFilePath, document.Payload);
                    documentViewerRichTextBox.SafeInvoke(_ => _.LoadFile(_tempFilePath));
                } else {
                    documentViewerRichTextBox.SafeInvoke(_ => _.Text = _current.Content);
                    SetDocumentViewStyle(_documentViewerTextFont, _documentViewerTextColor);
                }
            }, silent: true);
        }

        private void HighlightSearchTerms(params string[] terms) {
            foreach (var term in terms) {
                if (term.StartsWith("-")) { continue; }

                documentViewerRichTextBox.SafeInvoke(_ => _.Highlight(term));
            }
        }

        private void CopyTextToClipboard(string text) {
            if (string.IsNullOrWhiteSpace(text)) { return; }

            Clipboard.SetText(text);

            MessageBox.Show(Strings.TextCopiedToClipboard
                , Strings.Copy
                , MessageBoxButtons.OK
                , MessageBoxIcon.Information);
        }

        private void SetDocumentViewStyle(Font font, Color color) {
            documentViewerRichTextBox.SafeInvoke(_ => {
                _.SelectionStart = 0;
                _.SelectionLength = _.TextLength;

                _.SelectionFont = font;
                _.SelectionColor = color;

                _.SelectionStart = 0;
                _.SelectionLength = 0;
                _.Focus();
            });
        }

        private void ToggleImproveDocumentViewer() {
            _useImprovedViewOnDocumentViewer = !_useImprovedViewOnDocumentViewer;
            ShowDocumentInDocumentViewer();
            improvedViewButton.Image = _useImprovedViewOnDocumentViewer
                ? Images.improved_view_enabled_32x32
                : Images.improved_view_32x32;
        }

        #endregion Private Methods

        #region Event Handlers

        private void SearchResultForm_Load(object sender, EventArgs e) {
            Initialize();
        }

        private void SearchResultForm_FormClosing(object sender, FormClosingEventArgs e) {
            if (File.Exists(_tempFilePath)) { File.Delete(_tempFilePath); }
        }

        private void improvedViewButton_Click(object sender, EventArgs e) {
            ToggleImproveDocumentViewer();
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

            if (_position < SearchResult.Documents.Length - 1) {
                _position++;
            }
            ShowDocument(_position);
        }

        private void lastResultButton_Click(object sender, EventArgs e) {
            if (SearchResult == null) { return; }

            var button = sender as Button;
            if (button == null) { return; }

            _position = SearchResult.Documents.Length - 1;
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