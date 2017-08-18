using System;
using System.Windows.Forms;
using InfoFenix.Application.Code;
using InfoFenix.Application.Code.Helpers;
using InfoFenix.Application.Views.Shared;
using InfoFenix;
using InfoFenix.Services;
using InfoFenix.Resources;

namespace InfoFenix.Application.Views.Manage {

    public partial class ConfigurationForm : LayoutForm {

        #region Private Read-Only Fields

        private readonly IManagementService _managementService;

        #endregion Private Read-Only Fields

        #region Private Fields

        private bool _mustRestartApplication;

        #endregion Private Fields

        #region Public Constructors

        public ConfigurationForm(IManagementService managementService) {
            Prevent.ParameterNull(managementService, nameof(managementService));

            _managementService = managementService;

            InitializeComponent();
        }

        #endregion Public Constructors

        #region Private Methods

        private void NeedReboot() {
            if (_mustRestartApplication) {
                MessageBox.Show(Strings.MustRebootApplication, Strings.Reboot, MessageBoxButtons.OK, MessageBoxIcon.Information);
                EntryPoint.RestartApplication();
            }
        }

        #endregion Private Methods

        #region Event Handlers

        private void performDatabaseBackupButton_Click(object sender, EventArgs e) {
            var path = DialogHelper.OpenFolderBrowserDialog(Strings.ConfigurationForm_SelectDatabaseBackupDirectory);
            if (string.IsNullOrEmpty(path)) { return; }

            WaitFor.Execution(() => _managementService.BackupDatabase(path));
        }

        private void performDatabaseRestoreButton_Click(object sender, EventArgs e) {
            var paths = DialogHelper.OpenFileBrowserDialog(Strings.ConfigurationForm_SelectDatabaseRestoreFile);
            if (paths.IsNullOrEmpty()) { return; }

            WaitFor.Execution(() => _managementService.RestoreDatabase(paths[0]));

            _mustRestartApplication = true;

            NeedReboot();
        }

        private void performDatabaseCleanUpButton_Click(object sender, EventArgs e) {
            WaitFor.Execution(() => _managementService.CleanUpDatabase());
        }

        private void closeButton_Click(object sender, EventArgs e) {
            DialogResult = DialogResult.OK;
        }

        #endregion Event Handlers
    }
}