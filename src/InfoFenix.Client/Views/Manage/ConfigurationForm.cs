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
                MessageBox.Show(Resource.MustRebootApplication, Resource.Reboot, MessageBoxButtons.OK, MessageBoxIcon.Information);
                EntryPoint.RestartApplication();
            }
        }

        #endregion Private Methods

        #region Event Handlers

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
            DialogResult = DialogResult.OK;
        }

        #endregion Event Handlers
    }
}