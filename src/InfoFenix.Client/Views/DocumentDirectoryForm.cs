using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using InfoFenix.Client.Code;
using InfoFenix.Core;
using InfoFenix.Core.Entities;
using InfoFenix.Core.Infrastructure;
using InfoFenix.Core.Logging;
using InfoFenix.Core.Services;

namespace InfoFenix.Client.Views {

    public partial class DocumentDirectoryForm : Form {

        #region Private Read-Only Fields

        private readonly IDocumentDirectoryService _documentDirectoryService;
        private readonly ProgressiveTaskExecutor _progressiveTaskExecutor;

        #endregion Private Read-Only Fields

        #region Public Properties

        private ILogger _log;

        public ILogger Log {
            get { return _log ?? NullLogger.Instance; }
            set { _log = value ?? NullLogger.Instance; }
        }

        public DocumentDirectoryEntity ViewModel { get; set; }

        #endregion Public Properties

        #region Public Constructors

        public DocumentDirectoryForm(IDocumentDirectoryService documentDirectoryService, ProgressiveTaskExecutor progressiveTaskExecutor) {
            Prevent.ParameterNull(documentDirectoryService, nameof(documentDirectoryService));
            Prevent.ParameterNull(progressiveTaskExecutor, nameof(progressiveTaskExecutor));

            _documentDirectoryService = documentDirectoryService;
            _progressiveTaskExecutor = progressiveTaskExecutor;

            InitializeComponent();
        }

        #endregion Public Constructors

        #region Private Methods

        private void UpdateViewModel() {
            ViewModel = ViewModel ?? new DocumentDirectoryEntity();
            ViewModel.Label = documentDirectoryLabelTextBox.Text;
            ViewModel.Path = documentDirectoryPathTextBox.Text;
            ViewModel.Code = ViewModel.Code ?? Guid.NewGuid().ToString("N");
            ViewModel.Watch = watchDocumentDirectoryCheckBox.Checked;
            ViewModel.Index = indexDocumentDirectoryCheckBox.Checked;
        }

        private bool ValidateViewModel() {
            var errors = Validator.Validate(ViewModel);
            if (errors.IsValid) { return true; }
            MessageBox.Show(errors.First().Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }

        private void SaveDocumentDirectory() {
            _progressiveTaskExecutor.Execute((cancellationToken) => _documentDirectoryService.Save(ViewModel));
        }

        private void SaveDocumentsInsideDirectoryDocuments() {
            if (Directory.GetFiles(ViewModel.Path).Length > 0) {
                _progressiveTaskExecutor.Execute((cancellationToken) => _documentDirectoryService.SaveDocumentsInsideDocumentDirectoryAsync(ViewModel.ID, cancellationToken));
            }
        }

        private void CleanDocumentDirectory() {
            if (Directory.GetFiles(ViewModel.Path).Length > 0) {
                _progressiveTaskExecutor.Execute((cancellationToken) => _documentDirectoryService.CleanAsync(ViewModel.ID, cancellationToken));
            }
        }

        private void SelectDocumentDirectoryPath() {
            var path = DialogHelper.OpenFolderBrowserDialog("Selecionar o diretório de documentos");
            if (!string.IsNullOrWhiteSpace(path)) {
                documentDirectoryPathTextBox.Text = path;
            }
        }

        private void IndexDocumentDirectory(DocumentDirectoryEntity documentDirectory) {
            if (Directory.GetFiles(ViewModel.Path).Length > 0) {
                _progressiveTaskExecutor.Execute((cancellationToken) => _documentDirectoryService.IndexAsync(ViewModel.ID, cancellationToken));
            }
        }

        private void WatchDocumentDirectory(DocumentDirectoryEntity documentDirectory) {
            _progressiveTaskExecutor.Execute((cancellationToken) => _documentDirectoryService.WatchForModification(documentDirectory.ID));
        }

        #endregion Private Methods

        #region Event Handlers

        private void selectDocumentDirectoryPathButton_Click(object sender, EventArgs e) {
            var button = sender as Button;
            if (button == null) { return; }

            SelectDocumentDirectoryPath();
        }

        private void saveAndCloseButton_Click(object sender, EventArgs e) {
            var button = sender as Button;
            if (button == null) { return; }

            UpdateViewModel();
            if (!ValidateViewModel()) { return; }

            SaveDocumentDirectory();
            SaveDocumentsInsideDirectoryDocuments();
            CleanDocumentDirectory();

            if (ViewModel.Index) { IndexDocumentDirectory(ViewModel); }
            //if (ViewModel.Watch) { WatchDocumentDirectory(ViewModel); }

            DialogResult = DialogResult.OK;
        }

        #endregion Event Handlers
    }
}