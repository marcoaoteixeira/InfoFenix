using System;
using System.Windows.Forms;
using Resource = InfoFenix.Client.Properties.Resources;

namespace InfoFenix.Client.Views.Home {

    public partial class AboutForm : Form {

        #region Public Constructors

        public AboutForm() {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Public Static Methods

        public static void Open() {
            using (var form = new AboutForm()) { form.ShowDialog(); }
        }

        #endregion Public Static Methods

        #region Event Handlers

        private void AboutForm_Load(object sender, EventArgs e) {
            versionLabel.Text = string.Format(Resource.Display_Version,
                EntryPoint.ApplicationVersion.Major,
                EntryPoint.ApplicationVersion.Minor,
                EntryPoint.ApplicationVersion.Revision);
        }

        private void applicationIconPictureBox_Click(object sender, EventArgs e) {
            DialogResult = DialogResult.OK;
        }

        #endregion Event Handlers
    }
}