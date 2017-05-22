﻿using System;
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

        private void SaveDocumentDirectory() {
            _cqrsDispatcher.Command(new SaveDocumentDirectoryCommand {
                DocumentDirectoryID = ViewModel.DocumentDirectoryID,
                Label = ViewModel.Label,
                DirectoryPath = ViewModel.DirectoryPath,
                Code = ViewModel.Code,
                Watch = ViewModel.Watch,
                Index = ViewModel.Index
            });
        }

        private void SaveDocumentDirectoryDocuments() {
            _cqrsDispatcher.Command(new SaveDocumentDirectoryDocumentsCommand {
                DocumentDirectoryID = ViewModel.DocumentDirectoryID,
                DocumentDirectoryDirectoryPath = ViewModel.DirectoryPath
            });
        }

        private void SelectDocumentDirectoryPath() {
            var path = DialogHelper.OpenFolderBrowserDialog("Selecionar o diretório de documentos");
            if (!string.IsNullOrWhiteSpace(path)) {
                documentDirectoryPathTextBox.Text = path;
            }
        }

        private void IndexDocumentDirectory(DocumentDirectoryEntity documentDirectory) {
            using (var form = _formManager.Get<IndexDocumentDirectoryForm>(mdi: null, multipleInstance: false)) {
                form.SetViewModel(documentDirectory);
                form.ShowDialog();
            }
        }

        private void WatchDocumentDirectory(DocumentDirectoryEntity documentDirectory) {
            _cqrsDispatcher.Command(new WatchDocumentDirectoryCommand {
                DirectoryPath = documentDirectory.DirectoryPath
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
            if (ViewModel.Watch) { WatchDocumentDirectory(ViewModel); }

            DialogResult = DialogResult.OK;
        }

        #endregion Event Handlers
    }
}