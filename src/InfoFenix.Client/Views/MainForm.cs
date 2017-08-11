using System;
using System.Windows.Forms;
using InfoFenix.Client.Code;
using InfoFenix.Client.Views.DocumentDirectory;
using InfoFenix.Client.Views.Home;
using InfoFenix.Client.Views.Manage;
using InfoFenix.Client.Views.Search;
using InfoFenix.Core;
using Resource = InfoFenix.Client.Properties.Resources;

namespace InfoFenix.Client.Views {

    public partial class MainForm : Form {

        #region Private Read-Only Fields

        private readonly IFormManager _formManager;

        #endregion Private Read-Only Fields

        #region Public Constructors

        public MainForm(IFormManager formManager) {
            Prevent.ParameterNull(formManager, nameof(formManager));

            _formManager = formManager;

            InitializeComponent();
        }

        #endregion Public Constructors

        #region Event Handlers

        private void MainForm_Load(object sender, EventArgs e) {
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e) {
            if (EntryPoint.IgnoreExitRoutine) { return; }

            var response = MessageBox.Show(Resource.MainForm_Exit_Message
                , Resource.Exit
                , MessageBoxButtons.YesNo
                , MessageBoxIcon.Question);

            if (response == DialogResult.No) {
                e.Cancel = true;
                return;
            }
        }

        private void searchToolStripMenuItem_Click(object sender, EventArgs e) {
            _formManager.Get<SearchForm>(mdi: this, multipleInstance: false).Show();
        }

        private void documentDirectoryToolStripMenuItem_Click(object sender, EventArgs e) {
            _formManager.Get<ManageDocumentDirectoryForm>(mdi: this, multipleInstance: false).Show();
        }

        private void configurationToolStripMenuItem_Click(object sender, EventArgs e) {
            _formManager.Get<ConfigurationForm>(mdi: null, multipleInstance: false).ShowDialog();
        }

        private void helpInformationToolStripMenuItem_Click(object sender, EventArgs e) {
            _formManager.Get<HelpForm>(mdi: null, multipleInstance: false).Show();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e) {
            _formManager.Get<AboutForm>(mdi: null, multipleInstance: false).ShowDialog();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
            Close();
        }

        #endregion Event Handlers
    }
}