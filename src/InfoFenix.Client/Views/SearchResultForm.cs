using System;
using System.Windows.Forms;
using InfoFenix.Client.Views.Shared;

namespace InfoFenix.Client.Views {

    public partial class SearchResultForm : LayoutForm {

        #region Public Constructors

        public SearchResultForm() {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Event Handlers

        private void SearchResultForm_Resize(object sender, EventArgs e) {
            var form = sender as Form;
            if (form == null) { return; }

            var padding = Convert.ToInt32(Math.Round((webBrowserPanel.Width * .333f) / 2));
            webBrowserPanel.Padding = new Padding(padding, 10, padding, 10);
        }

        #endregion Event Handlers
    }
}