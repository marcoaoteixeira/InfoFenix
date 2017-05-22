using System;
using System.Reflection;
using System.Windows.Forms;

namespace InfoFenix.Client {
    public partial class SplashScreenForm : Form {
        #region Public Constructors
        public SplashScreenForm() {
            InitializeComponent();
        }
        #endregion

        #region Event Handlers
        private void SplashScreenForm_Load(object sender, EventArgs e) {
            var version = typeof(SplashScreenForm).Assembly.GetName().Version;

            versionLabel.Text = $"ver. {version.Major}.{version.Minor}.{version.Revision}";
        } 
        #endregion
    }
}
