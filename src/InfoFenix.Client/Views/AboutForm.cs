using System;
using System.Windows.Forms;

namespace InfoFenix.Client.Views {

    public partial class AboutForm : Form {

        #region Public Constructors

        public AboutForm() {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Event Handlers

        private void AboutForm_Load(object sender, EventArgs e) {
            versionLabel.Text = $"ver.{EntryPoint.ApplicationVersion.Major}.{EntryPoint.ApplicationVersion.Minor}.{EntryPoint.ApplicationVersion.Revision}";
        }

        private void closeButton_Click(object sender, EventArgs e) {
            DialogResult = DialogResult.OK;
        }

        #endregion Event Handlers
    }
}