using System;
using System.Linq;
using System.Windows.Forms;
using InfoFenix.Client.Code;
using InfoFenix.Core;
using InfoFenix.Core.Cqrs;
using InfoFenix.Core.Infrastructure;
using InfoFenix.Core.Entities;
using InfoFenix.Impl.Commands;

namespace InfoFenix.Client.Views {
    public partial class DocumentDirectoryForm : Form {
        #region Private Read-Only Fields

        private readonly ICqrsDispatcher _cqrsDispatcher;

        #endregion

        #region Public Properties

        public DocumentDirectoryEntity ViewModel {
            get { return Tag as DocumentDirectoryEntity; }
            set { Tag = value; }
        }   

        #endregion

        #region Public Constructors
        public DocumentDirectoryForm(ICqrsDispatcher cqrsDispatcher) {
            Prevent.ParameterNull(cqrsDispatcher, nameof(cqrsDispatcher));

            _cqrsDispatcher = cqrsDispatcher;

            InitializeComponent();
        }
        #endregion

        #region Private Methods

        private void SetViewModel() {
            if (ViewModel == null) {
                ViewModel = new DocumentDirectoryEntity();
            }
            ViewModel.Label = documentDirectoryLabelTextBox.Text;
            ViewModel.DirectoryPath = documentDirectoryPathTextBox.Text;
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

        private void SaveViewModel() {
            _cqrsDispatcher.Command(new SaveDocumentDirectoryCommand {
                DocumentDirectoryID = ViewModel.DocumentDirectoryID,
                Label = ViewModel.Label,
                DirectoryPath = ViewModel.DirectoryPath,
                Code = ViewModel.Code,
                Watch = ViewModel.Watch,
                Index = ViewModel.Index
            });
        }

        private void SelectDocumentDirectoryPath() {
            var path = DialogHelper.OpenFolderBrowserDialog("Selecionar o diretório de documentos");
            if (!string.IsNullOrWhiteSpace(path)) {
                documentDirectoryPathTextBox.Text = path;
            }
        }

        #endregion

        #region Event Handlers
        private void selectDocumentDirectoryPathButton_Click(object sender, EventArgs e) {
            var button = sender as Button;
            if (button == null) { return; }

            SelectDocumentDirectoryPath();
        }

        private void saveAndCloseButton_Click(object sender, EventArgs e) {
            var button = sender as Button;
            if (button == null) { return; }

            SetViewModel();
            if (!ValidateViewModel()) { return; }
            SaveViewModel();

            if (ViewModel.Watch) { }
            if (ViewModel.Index) { }

            DialogResult = DialogResult.OK;
        }

        #endregion
    }
}