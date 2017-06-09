using System;
using System.Windows.Forms;
using InfoFenix.Client.Code;
using InfoFenix.Client.Views;
using InfoFenix.Core;

namespace InfoFenix.Client {

    public partial class MainForm : Form {
        private readonly IFormManager _formManager;

        public MainForm(IFormManager formManager) {
            Prevent.ParameterNull(formManager, nameof(formManager));

            _formManager = formManager;

            InitializeComponent();
        }

        #region Event Handlers

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e) {
            if (EntryPoint.IgnoreExitRoutine) { return; }

            var response = MessageBox.Show("Deseja realmente sair do Info Fênix?", "Sair", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
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
            using (var form = _formManager.Get<ConfigurationForm>(mdi: null, multipleInstance: false)) {
                form.ShowDialog();
            }
        }

        private void helpInformationToolStripMenuItem_Click(object sender, EventArgs e) {
            _formManager.Get<HelpForm>(mdi: this, multipleInstance: false).Show();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e) {
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
            Close();
        }

        #endregion Event Handlers

        
    }
}