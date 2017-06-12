using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using InfoFenix.Client.Code;
using InfoFenix.Client.Views;
using InfoFenix.Core;
using InfoFenix.Core.PubSub;
using InfoFenix.Core.Services;

namespace InfoFenix.Client {

    public partial class MainForm : Form {

        #region Private Read-Only Fields

        private readonly IDocumentService _documentService;
        private readonly IFormManager _formManager;
        private readonly IPublisherSubscriber _publisherSubscriber;

        #endregion Private Read-Only Fields

        #region Private Fields

        private ISubscription<DirectoryContentChangeNotification> _directoryContentChangeSubscription;
        private ISubscription<ProcessDocumentInitializeNotification> _processDocumentInitializeSubscription;
        private ISubscription<ProcessDocumentCompleteNotification> _processDocumentCompleteSubscription;

        #endregion Private Fields

        #region Public Constructors

        public MainForm(IDocumentService documentService, IFormManager formManager, IPublisherSubscriber publisherSubscriber) {
            Prevent.ParameterNull(documentService, nameof(documentService));
            Prevent.ParameterNull(formManager, nameof(formManager));
            Prevent.ParameterNull(publisherSubscriber, nameof(publisherSubscriber));

            _documentService = documentService;
            _formManager = formManager;
            _publisherSubscriber = publisherSubscriber;

            InitializeComponent();
        }

        #endregion Public Constructors

        #region Private Methods

        private void SubscribeForNotifications() {
            _directoryContentChangeSubscription = _publisherSubscriber.Subscribe<DirectoryContentChangeNotification>(DirectoryContentChangeHandler);
            _processDocumentInitializeSubscription = _publisherSubscriber.Subscribe<ProcessDocumentInitializeNotification>(ProcessDocumentInitializeHandler);
            _processDocumentCompleteSubscription = _publisherSubscriber.Subscribe<ProcessDocumentCompleteNotification>(ProcessDocumentCompleteHandler);
        }

        private void UnsubscribeFromNotifications() {
            _publisherSubscriber.Unsubscribe(_directoryContentChangeSubscription);
        }

        private void DirectoryContentChangeHandler(DirectoryContentChangeNotification message) {
            Task.Run(() => _documentService.Process(message.WatchingPath, message.FullPath));
        }

        private void ProcessDocumentInitializeHandler(ProcessDocumentInitializeNotification message) {
            informationToolStripStatusLabel.Text = $"Processando: {message.FileName}";
        }

        private void ProcessDocumentCompleteHandler(ProcessDocumentCompleteNotification message) {
            informationToolStripStatusLabel.Text = string.Empty;
        }

        #endregion Private Methods

        #region Event Handlers

        private void MainForm_Load(object sender, EventArgs e) {
            SubscribeForNotifications();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e) {
            if (EntryPoint.IgnoreExitRoutine) { return; }

            var response = MessageBox.Show("Deseja realmente sair do Info Fênix?", "Sair", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (response == DialogResult.No) {
                e.Cancel = true;
                return;
            }

            //UnsubscribeFromNotifications();
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
            _formManager.Get<AboutForm>(mdi: null, multipleInstance: false).ShowDialog();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
            Close();
        }

        #endregion Event Handlers

        
    }
}