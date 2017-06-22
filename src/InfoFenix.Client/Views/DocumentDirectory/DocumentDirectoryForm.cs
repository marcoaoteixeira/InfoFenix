using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using InfoFenix.Client.Code;
using InfoFenix.Client.Code.Helpers;
using InfoFenix.Core;
using InfoFenix.Core.Commands;
using InfoFenix.Core.Cqrs;
using InfoFenix.Core.Dto;
using InfoFenix.Core.Infrastructure;
using InfoFenix.Core.Logging;

namespace InfoFenix.Client.Views.DocumentDirectory {

    public partial class DocumentDirectoryForm : Form {

        #region Private Read-Only Fields

        private readonly IMediator _mediator;
        //private readonly ProgressiveTaskExecutor _progressiveTaskExecutor;

        #endregion Private Read-Only Fields

        #region Public Properties

        private ILogger _log;

        public ILogger Log {
            get { return _log ?? NullLogger.Instance; }
            set { _log = value ?? NullLogger.Instance; }
        }

        public DocumentDirectoryDto ViewModel { get; set; }

        #endregion Public Properties

        #region Public Constructors

        public DocumentDirectoryForm(IMediator mediator/*, ProgressiveTaskExecutor progressiveTaskExecutor*/) {
            Prevent.ParameterNull(mediator, nameof(mediator));
            //Prevent.ParameterNull(progressiveTaskExecutor, nameof(progressiveTaskExecutor));

            _mediator = mediator;
            //_progressiveTaskExecutor = progressiveTaskExecutor;

            InitializeComponent();
        }

        #endregion Public Constructors

        #region Private Methods

        private void Initialize() {
            ViewModel = ViewModel ?? new DocumentDirectoryDto();
        }

        private void UpdateViewModel() {
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
            //_progressiveTaskExecutor.Execute((cancellationToken) => _mediator.CommandAsync(new SaveDocumentDirectoryCommand { DocumentDirectory = ViewModel }, cancellationToken));
        }

        private void SaveDocumentsInsideDirectoryDocuments() {
            if (Directory.GetFiles(ViewModel.Path).Length > 0) {
                //_progressiveTaskExecutor.Execute((cancellationToken) => _mediator.SaveDocumentsInsideDocumentDirectoryAsync(ViewModel.ID, cancellationToken));
            }
        }

        private void CleanDocumentDirectory() {
            if (Directory.GetFiles(ViewModel.Path).Length > 0) {
                //_progressiveTaskExecutor.Execute((cancellationToken) => _mediator.CleanAsync(ViewModel.ID, cancellationToken));
            }
        }

        private void SelectDocumentDirectoryPath() {
            var path = DialogHelper.OpenFolderBrowserDialog("Selecionar o diretório de documentos");
            if (!string.IsNullOrWhiteSpace(path)) {
                documentDirectoryPathTextBox.Text = path;
            }
        }

        private void IndexDocumentDirectory(DocumentDirectoryDto documentDirectory) {
            if (Directory.GetFiles(ViewModel.Path).Length > 0) {
                //_progressiveTaskExecutor.Execute((cancellationToken) => _mediator.IndexAsync(ViewModel.ID, cancellationToken));
            }
        }

        private void WatchDocumentDirectory(DocumentDirectoryDto documentDirectory) {
            //_progressiveTaskExecutor.Execute(() => _mediator.StartWatchForModification(documentDirectory.ID));
        }

        #endregion Private Methods

        #region Event Handlers

        private void DocumentDirectoryForm_Load(object sender, EventArgs e) {
            Initialize();
        }

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

            try { SaveDocumentDirectory(); } catch (Exception ex) {
                MessageBox.Show($"Ocorreu um erro ao tentar salvar o diretório de documentos. ({ex.Message})", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);

                DialogResult = DialogResult.Abort;
            }

            SaveDocumentsInsideDirectoryDocuments();
            CleanDocumentDirectory();

            if (ViewModel.Index) { IndexDocumentDirectory(ViewModel); }
            if (ViewModel.Watch) { WatchDocumentDirectory(ViewModel); }

            DialogResult = DialogResult.OK;
        }

        #endregion Event Handlers
    }
}