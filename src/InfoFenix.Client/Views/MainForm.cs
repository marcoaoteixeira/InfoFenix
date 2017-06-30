using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Forms;
using InfoFenix.Client.Code;
using InfoFenix.Client.Views.DocumentDirectory;
using InfoFenix.Client.Views.Home;
using InfoFenix.Client.Views.Manage;
using InfoFenix.Client.Views.Search;
using InfoFenix.Core;
using InfoFenix.Core.Commands;
using InfoFenix.Core.Cqrs;
using InfoFenix.Core.Dto;
using InfoFenix.Core.PubSub;
using InfoFenix.Core.Queries;
using Resource = InfoFenix.Client.Properties.Resources;

namespace InfoFenix.Client.Views {

    public partial class MainForm : Form {

        #region Private Read-Only Fields

        private readonly CancellationTokenIssuer _cancellationTokenIssuer;
        private readonly IFormManager _formManager;
        private readonly IMediator _mediator;
        private readonly IPublisherSubscriber _publisherSubscriber;

        #endregion Private Read-Only Fields

        #region Private Fields

        private ISubscription<DirectoryContentChangeNotification> _directoryContentChangeSubscription;
        private IProgress<ProgressInfo> _progress;

        #endregion Private Fields

        #region Public Constructors

        public MainForm(CancellationTokenIssuer cancellationTokenIssuer, IFormManager formManager, IMediator mediator, IPublisherSubscriber publisherSubscriber) {
            Prevent.ParameterNull(cancellationTokenIssuer, nameof(cancellationTokenIssuer));
            Prevent.ParameterNull(formManager, nameof(formManager));
            Prevent.ParameterNull(mediator, nameof(mediator));
            Prevent.ParameterNull(publisherSubscriber, nameof(publisherSubscriber));

            _cancellationTokenIssuer = cancellationTokenIssuer;
            _formManager = formManager;
            _mediator = mediator;
            _publisherSubscriber = publisherSubscriber;
            
            _progress = new Progress<ProgressInfo>(NotifyProcess);

            InitializeComponent();
            Initialize();
        }

        #endregion Public Constructors

        #region Private Methods

        private void Initialize() {
            SubscribeForNotifications();
        }

        private void SubscribeForNotifications() {
            _directoryContentChangeSubscription = _publisherSubscriber.Subscribe<DirectoryContentChangeNotification>(DirectoryContentChangeHandler);
        }

        private void UnsubscribeFromNotifications() {
            _publisherSubscriber.Unsubscribe(_directoryContentChangeSubscription);
        }

        private void DirectoryContentChangeHandler(DirectoryContentChangeNotification message) {
            if (message.Changes == DirectoryContentChangeNotification.ChangeTypes.Created) {
                ProcessDocumentDirectoryChange_Create(message.WatchingPath, message.FullPath, _progress);
            }
        }

        private void ProcessDocumentDirectoryChange_Create(string watchingPath, string fullPath, IProgress<ProgressInfo> progress) {
            Task.Run(() => {
                var document = _mediator.Query(new GetDocumentDirectoryContentChangeQuery {
                    DocumentDirectoryPath = watchingPath,
                    DocumentPath = fullPath
                });

                _mediator.Command(new SaveDocumentCommand {
                    Document = document
                }, _progress);

                _mediator.Command(new IndexDocumentCommand {
                    IndexName = document.DocumentDirectory.Code,
                    DocumentIndex = DocumentIndexDto.Map(document)
                }, _progress);
            });
        }

        private void NotifyProcess(ProgressInfo info) {
            switch (info.State) {
                case ProgressState.PerformStep:
                    informationToolStripStatusLabel.Text = info.Message;
                    break;

                case ProgressState.Cancel:
                case ProgressState.Complete:
                case ProgressState.Error:
                    informationToolStripStatusLabel.Text = string.Empty;
                    break;
            }
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