using System;
using System.Windows.Forms;
using InfoFenix.Client.Code;
using InfoFenix.Client.Views.DocumentDirectory;
using InfoFenix.Client.Views.Home;
using InfoFenix.Client.Views.Manage;
using InfoFenix.Client.Views.Search;
using InfoFenix.Core;
using InfoFenix.Core.PubSub;
using Resource = InfoFenix.Client.Properties.Resources;

namespace InfoFenix.Client.Views {

    public partial class MainForm : Form {

        #region Private Read-Only Fields

        private readonly CancellationTokenIssuer _cancellationTokenIssuer;
        private readonly IFormManager _formManager;
        private readonly IPublisherSubscriber _publisherSubscriber;

        #endregion Private Read-Only Fields

        #region Private Fields

        private ISubscription<DirectoryContentChangeNotification> _directoryContentChangeSubscription;

        #endregion Private Fields

        #region Public Constructors

        public MainForm(CancellationTokenIssuer cancellationTokenIssuer, IFormManager formManager, IPublisherSubscriber publisherSubscriber) {
            Prevent.ParameterNull(cancellationTokenIssuer, nameof(cancellationTokenIssuer));
            Prevent.ParameterNull(formManager, nameof(formManager));
            Prevent.ParameterNull(publisherSubscriber, nameof(publisherSubscriber));

            _cancellationTokenIssuer = cancellationTokenIssuer;
            _formManager = formManager;
            _publisherSubscriber = publisherSubscriber;

            SubscribeForNotifications();

            InitializeComponent();
        }

        #endregion Public Constructors

        #region Private Methods

        private void SubscribeForNotifications() {
            _directoryContentChangeSubscription = _publisherSubscriber.Subscribe<DirectoryContentChangeNotification>(DirectoryContentChangeHandler);
        }

        private void UnsubscribeFromNotifications() {
            _publisherSubscriber.Unsubscribe(_directoryContentChangeSubscription);
        }

        private void DirectoryContentChangeHandler(DirectoryContentChangeNotification message) {

        }

        #endregion Private Methods

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

            UnsubscribeFromNotifications();
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