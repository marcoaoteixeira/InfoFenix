using System;
using System.IO;
using InfoFenix.Client.Views.Shared;
using InfoFenix.Core;

namespace InfoFenix.Client.Views {

    public partial class HelpForm : LayoutForm {

        #region Private Static Read-Only Fields

        private static readonly string HomePage = Path.Combine(Common.ApplicationDirectoryPath, "Help", "index.html");

        #endregion

        #region Public Constructors

        public HelpForm() {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Event Handlers

        private void HelpForm_Load(object sender, EventArgs e) {
            mainWebBrowser.Navigate(HomePage);
        }

        private void closeButton_Click(object sender, EventArgs e) {
            Close();
        }

        #endregion
    }
}