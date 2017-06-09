using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using InfoFenix.Client.Models;
using InfoFenix.Client.Views.Shared;
using InfoFenix.Core;
using InfoFenix.Core.Office;
using Spire.Doc;
using Spire.DocViewer.Forms;

namespace InfoFenix.Client.Views {

    public partial class SearchResultForm : LayoutForm {

        #region Private Read-Only Fields

        private readonly string _tempFilePath = Path.Combine(Path.GetTempPath(), Path.GetTempFileName());
        private readonly IWordApplication _wordApplication;

        #endregion Private Read-Only Fields

        #region Private Fields

        private int _position;
        private string _currentText;

        #endregion Private Fields

        #region Public Properties

        public string[] SearchTerms { get; set; }

        public SearchDocumentDirectoryViewModel SearchResult { get; set; }

        #endregion Public Properties

        #region Public Constructors

        public SearchResultForm(IWordApplication wordApplication) {
            _wordApplication = wordApplication;

            InitializeComponent();

            /*
            SearchTerms = new[] { "AÇÃO" };
            SearchResult = new SearchDocumentDirectoryViewModel {
                Documents = new List<SearchDocumentViewModel> {
                    new SearchDocumentViewModel {
                        Path = @"C:\Workspace\VS2017\InfoFenix\resources\votos\DrRenatoSartorelli\512\256\128\rs8700.doc",
                        Payload = File.ReadAllBytes(@"C:\Workspace\VS2017\InfoFenix\resources\votos\DrRenatoSartorelli\512\256\128\rs8700.doc")
                    },
                    new SearchDocumentViewModel {
                        Path=@"C:\Workspace\VS2017\InfoFenix\resources\votos\DrRenatoSartorelli\512\256\128\rs8702.doc",
                        Payload = File.ReadAllBytes(@"C:\Workspace\VS2017\InfoFenix\resources\votos\DrRenatoSartorelli\512\256\128\rs8702.doc")
                    },
                    new SearchDocumentViewModel {
                        Path=@"C:\Workspace\VS2017\InfoFenix\resources\votos\DrRenatoSartorelli\512\256\128\rs8703.doc",
                        Payload = File.ReadAllBytes(@"C:\Workspace\VS2017\InfoFenix\resources\votos\DrRenatoSartorelli\512\256\128\rs8703.doc")
                    },
                    new SearchDocumentViewModel {
                        Path = @"C:\Workspace\VS2017\InfoFenix\resources\votos\DrRenatoSartorelli\512\256\128\rs8705.doc",
                        Payload = File.ReadAllBytes(@"C:\Workspace\VS2017\InfoFenix\resources\votos\DrRenatoSartorelli\512\256\128\rs8705.docx")
                    },
                    new SearchDocumentViewModel {
                        Path = @"C:\Workspace\VS2017\InfoFenix\resources\votos\DrRenatoSartorelli\512\256\128\rs8706.doc",
                        Payload = File.ReadAllBytes(@"C:\Workspace\VS2017\InfoFenix\resources\votos\DrRenatoSartorelli\512\256\128\rs8706.doc")
                    }
                }
            };
            */
        }

        #endregion Public Constructors

        #region Private Methods

        private void Initialize() {
            informationLabel.Text = string.Empty;

            documentDocumentViewer.ZoomMode = ZoomMode.FitWidth;
            documentDocumentViewer.EnableHandTools = true;

            ShowDocument(0); // Show first document.
        }

        private void ShowDocument(int index) {
            if (SearchResult == null) { return; }

            var document = SearchResult.Documents[index];

            informationLabel.Text = $"Documento: {index + 1} de {SearchResult.Documents.Count}";
            documentPathLabel.Text = $"Nome do documento: {Path.GetFileNameWithoutExtension(document.Path)}";

            using (var inputStream = new MemoryStream(document.Payload))
            using (var outputStream = new FileStream(_tempFilePath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite)) {
                if (!SearchTerms.IsNullOrEmpty()) {
                    using (var doc = _wordApplication.Open(inputStream)) {
                        _currentText = doc.Text;

                        doc.Highlight(outputStream, SearchTerms);
                    }
                }
            }

            documentDocumentViewer.LoadFromFile(_tempFilePath, FileFormat.Doc);
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

            if (string.IsNullOrWhiteSpace(_currentText)) { return; }

            Clipboard.SetText(_currentText);

            MessageBox.Show("Texto selecionado foi copiado para a área de transferência"
                , "Copiar"
                , MessageBoxButtons.OK
                , MessageBoxIcon.Information);
        }

        #endregion Event Handlers
    }
}