using System;
using System.IO;
using System.Windows.Forms;
using InfoFenix.Core;
using InfoFenix.Core.Commands;
using InfoFenix.Core.Cqrs;
using InfoFenix.Core.Entities;
using InfoFenix.Core.Logging;
using InfoFenix.Core.PubSub;

namespace InfoFenix.Client.Views {

    public partial class IndexDocumentDirectoryForm : Form {

        #region Private Read-Only Fields

        private readonly CancellationTokenIssuer _issuer;
        private readonly ICqrsDispatcher _cqrsDispatcher;
        private readonly IPublisherSubscriber _pubSub;

        #endregion Private Read-Only Fields

        #region Private Fields

        private DocumentDirectoryEntity _viewModel;

        #endregion

        #region Public Properties

        private ILogger _log;
        public ILogger Log {
            get { return _log ?? NullLogger.Instance; }
            set { _log = value ?? NullLogger.Instance; }
        }

        #endregion

        #region Private Fields

        private ISubscription<StartingDocumentDirectoryIndexingNotification> _startingDocumentDirectoryIndexingNotification;
        private ISubscription<StartingDocumentIndexingNotification> _startingDocumentIndexingNotification;
        private ISubscription<DocumentIndexingCompleteNotification> _documentIndexingCompleteNotification;
        private ISubscription<DocumentDirectoryIndexingCompleteNotification> _documentDirectoryIndexingCompleteNotification;

        private bool _indexing;

        #endregion Private Fields

        #region Public Constructors

        public IndexDocumentDirectoryForm(CancellationTokenIssuer issuer, ICqrsDispatcher cqrsDispatcher, IPublisherSubscriber pubSub) {
            Prevent.ParameterNull(issuer, nameof(issuer));
            Prevent.ParameterNull(cqrsDispatcher, nameof(cqrsDispatcher));
            Prevent.ParameterNull(pubSub, nameof(pubSub));

            _issuer = issuer;
            _cqrsDispatcher = cqrsDispatcher;
            _pubSub = pubSub;

            SubscribeForNotification();

            InitializeComponent();
        }

        #endregion Public Constructors

        #region Public Methods

        public void SetViewModel(DocumentDirectoryEntity documentDirectory) {
            _viewModel = documentDirectory;
        }

        #endregion

        #region Private Methods

        private void Initialize() {
            if (_viewModel == null) { return; }

            documentLabel.Text = string.Empty;
            fromToLabel.Text = string.Empty;
            stopButton.Enabled = true;

            _indexing = true;

            _cqrsDispatcher.Command(new IndexDocumentDirectoryCommand {
                DocumentDirectoryID = _viewModel.DocumentDirectoryID,
                Code = _viewModel.Code,
                DirectoryPath = _viewModel.DirectoryPath
            });
        }

        private void StopDocumentDirectoryIndexing() {
            if (!_indexing) { return; }

            try { _issuer.Cancel(_viewModel.Code); }
            catch (Exception ex) { Log.Error(ex, ex.Message); }
            finally { _indexing = false; }

            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void SubscribeForNotification() {
            _startingDocumentDirectoryIndexingNotification = _pubSub.Subscribe<StartingDocumentDirectoryIndexingNotification>(StartingDocumentDirectoryIndexingHandler);
            _startingDocumentIndexingNotification = _pubSub.Subscribe<StartingDocumentIndexingNotification>(StartingDocumentIndexingHandler);
            _documentIndexingCompleteNotification = _pubSub.Subscribe<DocumentIndexingCompleteNotification>(DocumentIndexingCompleteHandler);
            _documentDirectoryIndexingCompleteNotification = _pubSub.Subscribe<DocumentDirectoryIndexingCompleteNotification>(DocumentDirectoryIndexingCompleteHandler);
        }

        private void UnsubscribeFromNotification() {
            _pubSub.Unsubscribe(_startingDocumentDirectoryIndexingNotification);
            _pubSub.Unsubscribe(_startingDocumentIndexingNotification);
            _pubSub.Unsubscribe(_documentIndexingCompleteNotification);
            _pubSub.Unsubscribe(_documentDirectoryIndexingCompleteNotification);
        }

        private void StartingDocumentDirectoryIndexingHandler(StartingDocumentDirectoryIndexingNotification message) {
            if (indexingProgressBar.InvokeRequired) {
                indexingProgressBar.Invoke((MethodInvoker)delegate { indexingProgressBar.Maximum = message.TotalDocuments; });
            } else {
                indexingProgressBar.Maximum = message.TotalDocuments;
            }
        }

        private void StartingDocumentIndexingHandler(StartingDocumentIndexingNotification message) {
            if (documentLabel.InvokeRequired) {
                documentLabel.Invoke((MethodInvoker)delegate { documentLabel.Text = Path.GetFileName(message.FullPath); });
            } else {
                documentLabel.Text = Path.GetFileName(message.FullPath);
            }

            if (fromToLabel.InvokeRequired) {
                fromToLabel.Invoke((MethodInvoker)delegate { fromToLabel.Text = $"{indexingProgressBar.Step} de {message.TotalDocuments}"; });
            } else {
                fromToLabel.Text = $"{indexingProgressBar.Step} de {message.TotalDocuments}";
            }
        }

        private void DocumentIndexingCompleteHandler(DocumentIndexingCompleteNotification message) {
            if (indexingProgressBar.InvokeRequired) {
                indexingProgressBar.Invoke((MethodInvoker)delegate { indexingProgressBar.PerformStep(); });
            } else {
                indexingProgressBar.PerformStep();
            }
        }

        private void DocumentDirectoryIndexingCompleteHandler(DocumentDirectoryIndexingCompleteNotification message) {
            _indexing = false;

            if (InvokeRequired) {
                Invoke((MethodInvoker)delegate {
                    DialogResult = DialogResult.OK;
                    Close();
                });
            } else {
                DialogResult = DialogResult.OK;
                Close();
            }
        }

        

        #endregion Private Methods

        #region Event Handlers

        private void IndexDocumentDirectoryForm_Load(object sender, EventArgs e) {
            Initialize();
        }

        private void IndexDocumentDirectoryForm_FormClosing(object sender, FormClosingEventArgs e) {
            if (_indexing) {
                var result = MessageBox.Show("Deseja realmente parar a indexação de documentos?", "Indexação", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result != DialogResult.Yes) { e.Cancel = true; return; }
                else { StopDocumentDirectoryIndexing();  }
            }

            UnsubscribeFromNotification();
        }

        private void stopButton_Click(object sender, EventArgs e) {
            StopDocumentDirectoryIndexing();
        }

        #endregion Event Handlers
    }
}