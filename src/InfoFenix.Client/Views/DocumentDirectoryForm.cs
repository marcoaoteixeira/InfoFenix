using System;
using System.Linq;
using System.Windows.Forms;
using InfoFenix.Client.Code;
using InfoFenix.Core;
using InfoFenix.Core.Cqrs;
using InfoFenix.Core.Entities;
using InfoFenix.Core.Infrastructure;
using InfoFenix.Core.Commands;

namespace InfoFenix.Client.Views {

    public partial class DocumentDirectoryForm : Form {

        #region Private Read-Only Fields

        private readonly ICqrsDispatcher _cqrsDispatcher;
        private readonly IFormManager _formManager;

        #endregion Private Read-Only Fields

        #region Public Properties

        public DocumentDirectoryEntity ViewModel { get; set; }

        #endregion Public Properties

        #region Public Constructors

        public DocumentDirectoryForm(ICqrsDispatcher cqrsDispatcher, IFormManager formManager) {
            Prevent.ParameterNull(cqrsDispatcher, nameof(cqrsDispatcher));
            Prevent.ParameterNull(formManager, nameof(formManager));

            _cqrsDispatcher = cqrsDispatcher;
            _formManager = formManager;

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
            var command = new SaveDocumentDirectoryCommand {
                DocumentDirectoryID = ViewModel.ID,
                Label = ViewModel.Label,
                DirectoryPath = ViewModel.Path,
                Code = ViewModel.Code,
                Watch = ViewModel.Watch,
                Index = ViewModel.Index
            };
            _cqrsDispatcher.Command(command);
            ViewModel.ID = command.DocumentDirectoryID;
        }

        private void SaveDocumentDirectoryDocuments() {
            using (var form = _formManager.Get<ProgressForm>(mdi: null, multipleInstance: false)) {
                form.Task = new SaveDocumentsInDocumentDirectoryCommand {
                    DocumentDirectoryID = ViewModel.ID,
                    DocumentDirectoryPath = ViewModel.Path,
                    DocumentDirectoryCode = ViewModel.Code
                };
                form.ShowDialog();
            }
        }

        private void SelectDocumentDirectoryPath() {
            var path = DialogHelper.OpenFolderBrowserDialog("Selecionar o diretório de documentos");
            if (!string.IsNullOrWhiteSpace(path)) {
                documentDirectoryPathTextBox.Text = path;
            }
        }

        private void IndexDocumentDirectory(DocumentDirectoryEntity documentDirectory) {
            using (var form = _formManager.Get<ProgressForm>(mdi: null, multipleInstance: false)) {
                form.Task = new IndexDocumentDirectoryCommand {
                    DocumentDirectoryCode = ViewModel.Code,
                    DocumentDirectoryPath = ViewModel.Path,
                    DocumentDirectoryID = ViewModel.ID
                };
                form.ShowDialog();
            }
        }

        private void WatchDocumentDirectory(DocumentDirectoryEntity documentDirectory) {
            _cqrsDispatcher.Command(new StartWatchDocumentDirectoryCommand {
                DirectoryPath = documentDirectory.Path
            });
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
            SaveDocumentDirectoryDocuments();
            
            if (ViewModel.Index) { IndexDocumentDirectory(ViewModel); }
            //if (ViewModel.Watch) { WatchDocumentDirectory(ViewModel); }

            DialogResult = DialogResult.OK;
        }

        #endregion Event Handlers
    }
}