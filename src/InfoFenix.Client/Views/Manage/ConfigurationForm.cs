using System;
using System.Windows.Forms;
using InfoFenix.Client.Code;
using InfoFenix.Client.Code.Helpers;
using InfoFenix.Client.Views.Shared;
using InfoFenix.Core;
using InfoFenix.Core.Services;
using Resource = InfoFenix.Client.Properties.Resources;

namespace InfoFenix.Client.Views.Manage {

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
                MessageBox.Show(Resource.ConfigurationForm_MustSelectApplicationDataDirectory, Resource.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void UpdateAppSettings() {
            _appSettings.Save();
        }

        private void NeedReboot() {
            if (_mustRestartApplication) {
                MessageBox.Show(Resource.MustRebootApplication, Resource.Reboot, MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            var path = DialogHelper.OpenFolderBrowserDialog(Resource.ConfigurationForm_SelectApplicationDataDirectory, Common.ApplicationDirectoryPath);
            if (!string.IsNullOrWhiteSpace(path)) {
                _appSettings.ApplicationDataDirectoryPath = path;
            }
        }

        private void performDatabaseBackupButton_Click(object sender, EventArgs e) {
            var path = DialogHelper.OpenFolderBrowserDialog(Resource.ConfigurationForm_SelectDatabaseBackupDirectory);
            if (string.IsNullOrEmpty(path)) { return; }

            WaitFor.Execution(() => _managementService.BackupDatabase(path));
        }

        private void performDatabaseRestoreButton_Click(object sender, EventArgs e) {
            var paths = DialogHelper.OpenFileBrowserDialog(Resource.ConfigurationForm_SelectDatabaseRestoreFile);
            if (paths.IsNullOrEmpty()) { return; }

            WaitFor.Execution(() => _managementService.RestoreDatabase(paths[0]));

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