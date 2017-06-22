using System;
using System.IO;
using System.Windows.Forms;
using InfoFenix.Client.Views.Shared;
using InfoFenix.Core.Dto;
using InfoFenix.Core.Office;
using Resource = InfoFenix.Client.Properties.Resources;

namespace InfoFenix.Client.Views.Search {

    public partial class SearchResultForm : LayoutForm {

        #region Private Read-Only Fields

        private readonly string _tempFilePath = Path.Combine(Path.GetTempPath(), Path.GetTempFileName());
        private readonly IWordApplication _wordApplication;

        #endregion Private Read-Only Fields

        #region Private Fields

        private int _position;

        #endregion Private Fields

        #region Public Properties

        public string[] SearchTerms { get; set; }

        public IndexDto SearchResult { get; set; }

        #endregion Public Properties

        #region Public Constructors

        public SearchResultForm(IWordApplication wordApplication) {
            _wordApplication = wordApplication;

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

            var document = SearchResult.Documents[index];

            documentViewerRichTextBox.Text = document.Content;

            informationLabel.Text = string.Format(Resource.SearchResultForm_InformationLabel, index + 1, SearchResult.Documents.Count);
            documentPathLabel.Text = string.Format(Resource.SearchResultForm_DocumentPathLabel, document.FileName);
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

            documentViewerRichTextBox.Text = "It works!";
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

            if (_position < SearchResult.Documents.Count - 1) {
                _position++;
            }
            ShowDocument(_position);
        }

        private void lastResultButton_Click(object sender, EventArgs e) {
            if (SearchResult == null) { return; }

            var button = sender as Button;
            if (button == null) { return; }

            _position = SearchResult.Documents.Count - 1;
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