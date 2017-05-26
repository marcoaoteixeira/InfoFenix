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

        private void documentDirectoryToolStripMenuItem_Click(object sender, EventArgs e) {
            _formManager.Get<ManageDocumentDirectoryForm>(mdi: this, multipleInstance: false).Show();
        }

        private void searchToolStripMenuItem_Click(object sender, EventArgs e) {
            using (var form = _formManager.Get<ProgressForm>(mdi: null, multipleInstance: false)) {
                form.ShowDialog();
            }
        }
    }
}