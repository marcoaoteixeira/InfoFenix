﻿using System;
using System.Windows.Forms;
using InfoFenix.Client.Code;
using InfoFenix.Client.Views.Shared;
using InfoFenix.Core;
using InfoFenix.Core.Services;

namespace InfoFenix.Client.Views {

    public partial class ConfigurationForm : LayoutForm {

        #region Private Read-Only Fields

        private readonly IAppSettings _appSettings;
        private readonly IManagementService _managementService;

        #endregion Private Read-Only Fields

        #region Private Fields

        private bool _mustRestartApplication;

        #endregion Private Fields

        #region Public Constructors

        public ConfigurationForm(IAppSettings appSettings, IManagementService managementService) {
            Prevent.ParameterNull(appSettings, nameof(appSettings));
            Prevent.ParameterNull(managementService, nameof(managementService));

            _appSettings = appSettings;
            _managementService = managementService;

            InitializeComponent();
        }

        #endregion Public Constructors

        #region Private Methods

        private void Initialize() {
            _appSettings.NotifyChange += AppSettings_NotifyChange;

            useRemoteDatabaseCheckBox.Checked = _appSettings.UseRemoteSearchDatabase;

            remoteDatabaseDirectoryPathTextBox.Enabled = _appSettings.UseRemoteSearchDatabase;
            remoteDatabaseDirectoryPathTextBox.Text = _appSettings.ApplicationDataDirectoryPath;

            selectRemoteDatabaseDirectoryPathButton.Enabled = _appSettings.UseRemoteSearchDatabase;
        }

        private void AppSettings_NotifyChange() {
            _mustRestartApplication = true;
        }

        private bool ValidateAppSettings() {
            if (useRemoteDatabaseCheckBox.Checked && string.IsNullOrWhiteSpace(remoteDatabaseDirectoryPathTextBox.Text)) {
                MessageBox.Show("É obrigatório selecionar a pasta da base de dados remota, se utilizar o modo remoto.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void UpdateAppSettings() {
            _appSettings.Save();
        }

        private void NeedReboot() {
            if (_mustRestartApplication) {
                MessageBox.Show("É necessário reiniciar a aplicação.", "Reinicar", MessageBoxButtons.OK, MessageBoxIcon.Information);
                EntryPoint.RestartApplication();
            }
        }

        #endregion Private Methods

        #region Event Handlers

        private void ConfigurationForm_Load(object sender, EventArgs e) {
            Initialize();
        }

        private void useRemoteDatabaseCheckBox_CheckedChanged(object sender, EventArgs e) {
            if (sender is CheckBox checkBox) {
                _appSettings.UseRemoteSearchDatabase = checkBox.Checked;
                remoteDatabaseDirectoryPathTextBox.Enabled = checkBox.Checked;
                selectRemoteDatabaseDirectoryPathButton.Enabled = checkBox.Checked;
            }
        }

        private void remoteDatabaseDirectoryPathTextBox_Leave(object sender, EventArgs e) {
            if (sender is TextBox textBox) {
                _appSettings.ApplicationDataDirectoryPath = textBox.Text;
            }
        }

        private void remoteDatabaseDirectoryPathTextBox_TextChanged(object sender, EventArgs e) {
            if (sender is TextBox textBox) {
                _appSettings.ApplicationDataDirectoryPath = textBox.Text;
            }
        }

        private void selectRemoteDatabaseDirectoryPathButton_Click(object sender, EventArgs e) {
            var path = DialogHelper.OpenFolderBrowserDialog("Selecionar diretório da base de dados", Common.ApplicationDirectoryPath);
            if (!string.IsNullOrWhiteSpace(path)) {
                _appSettings.ApplicationDataDirectoryPath = path;
            }
        }

        private void performDatabaseBackupButton_Click(object sender, EventArgs e) {
            var path = DialogHelper.OpenFolderBrowserDialog("Selecionar diretório de destino");
            if (string.IsNullOrEmpty(path)) { return; }

            WaitForm.WaitFor(() => _managementService.BackupDatabase(path));
        }

        private void performDatabaseRestoreButton_Click(object sender, EventArgs e) {
            var paths = DialogHelper.OpenFileBrowserDialog("Selecionar arquivo de restauração");
            if (paths.IsNullOrEmpty()) { return; }

            WaitForm.WaitFor(() => _managementService.RestoreDatabase(paths[0]));

            _mustRestartApplication = true;

            NeedReboot();
        }

        private void closeButton_Click(object sender, EventArgs e) {
            if (!ValidateAppSettings()) { return; }

            UpdateAppSettings();

            NeedReboot();

            DialogResult = DialogResult.OK;
        }

        #endregion Event Handlers
    }
}