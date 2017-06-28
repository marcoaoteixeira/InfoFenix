using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using InfoFenix.Client.Code;
using InfoFenix.Client.Code.Helpers;
using InfoFenix.Client.Views.Shared;
using InfoFenix.Core;
using InfoFenix.Core.Commands;
using InfoFenix.Core.Cqrs;
using InfoFenix.Core.Dto;
using InfoFenix.Core.Infrastructure;
using InfoFenix.Core.Logging;
using InfoFenix.Core.PubSub;
using InfoFenix.Core.Queries;
using Resource = InfoFenix.Client.Properties.Resources;

namespace InfoFenix.Client.Views.DocumentDirectory {

    public partial class DocumentDirectoryForm : Form {

        #region Private Read-Only Fields

        private readonly CancellationTokenIssuer _cancellationTokenIssuer;
        private readonly IMediator _mediator;
        private readonly IPublisherSubscriber _publisherSubscriber;

        #endregion Private Read-Only Fields

        #region Public Properties

        private ILogger _log;

        public ILogger Log {
            get { return _log ?? NullLogger.Instance; }
            set { _log = value ?? NullLogger.Instance; }
        }

        public DocumentDirectoryDto ViewModel { get; set; }

        #endregion Public Properties

        #region Public Constructors

        public DocumentDirectoryForm(CancellationTokenIssuer cancellationTokenIssuer, IMediator mediator, IPublisherSubscriber publisherSubscriber) {
            Prevent.ParameterNull(cancellationTokenIssuer, nameof(cancellationTokenIssuer));
            Prevent.ParameterNull(mediator, nameof(mediator));
            Prevent.ParameterNull(publisherSubscriber, nameof(publisherSubscriber));

            _cancellationTokenIssuer = cancellationTokenIssuer;
            _mediator = mediator;
            _publisherSubscriber = publisherSubscriber;

            InitializeComponent();
        }

        #endregion Public Constructors

        #region Private Methods

        private void InitializeDefaultState() {
            ViewModel = ViewModel ?? new DocumentDirectoryDto {
                Code = Guid.NewGuid().ToString("N"),
                Watch = true,
                Index = true
            };

            documentDirectoryLabelTextBox.Text = ViewModel.Label;
            documentDirectoryPathTextBox.Text = ViewModel.Path;
            watchDocumentDirectoryCheckBox.Checked = ViewModel.Watch;
            indexDocumentDirectoryCheckBox.Checked = ViewModel.Index;
        }

        private bool ValidateViewModel() {
            var errors = Validator.Validate(ViewModel);
            if (errors.IsValid) { return true; }
            MessageBox.Show(errors[0].Message, Resource.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }

        private void SaveDocumentDirectory(CancellationToken cancellationToken, IProgress<ProgressInfo> progress) {
            var documentDirectory = _mediator
                .Query(new GetDocumentDirectoryByPathQuery {
                    Path = ViewModel.Path,
                    RequireDocuments = true
                });

            if (documentDirectory != null) {
                ViewModel.DocumentDirectoryID = documentDirectory.DocumentDirectoryID;
                ViewModel.Documents = documentDirectory.Documents;
            }

            _mediator
                .CommandAsync(new SaveDocumentDirectoryCommand {
                    DocumentDirectory = ViewModel
                }, cancellationToken, progress).Wait();
        }

        private void SaveDocumentCollection(CancellationToken cancellationToken, IProgress<ProgressInfo> progress) {
            _mediator
                .CommandAsync(new SaveDocumentCollectionInDocumentDirectoryCommand {
                    DocumentDirectory = ViewModel,
                }, cancellationToken, progress).Wait();
        }

        private void CleanDocumentDirectory(CancellationToken cancellationToken, IProgress<ProgressInfo> progress) {
            _mediator
                .CommandAsync(new CleanDocumentDirectoryCommand {
                    DocumentDirectory = ViewModel
                }, cancellationToken, progress).Wait();
        }

        private void IndexDocumentDirectory(CancellationToken cancellationToken, IProgress<ProgressInfo> progress) {
            if (!ViewModel.Index) { return; }

            _mediator
                .CommandAsync(new IndexDocumentCollectionCommand {
                    BatchSize = 128,
                    Documents = ViewModel.Documents.Where(_ => !_.Indexed).Select(DocumentIndexDto.Map).ToList(),
                    IndexName = ViewModel.Code,
                }, cancellationToken, progress).Wait();
        }

        private void WatchDocumentDirectory(CancellationToken cancellationToken, IProgress<ProgressInfo> progress) {
            ICommand command;
            if (ViewModel.Watch) { command = new StartWatchDocumentDirectoryCommand { DocumentDirectory = ViewModel }; } else { command = new StopWatchDocumentDirectoryCommand { DocumentDirectory = ViewModel }; }

            _mediator
                .CommandAsync(command, cancellationToken, progress).Wait();
        }

        private void SelectDocumentDirectoryPath() {
            var path = DialogHelper.OpenFolderBrowserDialog(Resource.DocumentDirectoryForm_SelectDocumentDirectoryPath_Message);
            if (!string.IsNullOrWhiteSpace(path)) {
                documentDirectoryPathTextBox.Text = path;
            }
        }

        #endregion Private Methods

        #region Event Handlers

        private void DocumentDirectoryForm_Load(object sender, EventArgs e) {
            InitializeDefaultState();
        }

        private void documentDirectoryLabelTextBox_Leave(object sender, EventArgs e) {
            if (sender is TextBox textBox) { ViewModel.Label = textBox.Text; }
        }

        private void documentDirectoryLabelTextBox_TextChanged(object sender, EventArgs e) {
            if (sender is TextBox textBox) { ViewModel.Label = textBox.Text; }
        }

        private void documentDirectoryPathTextBox_Leave(object sender, EventArgs e) {
            if (sender is TextBox textBox) { ViewModel.Path = textBox.Text; }
        }

        private void documentDirectoryPathTextBox_TextChanged(object sender, EventArgs e) {
            if (sender is TextBox textBox) { ViewModel.Path = textBox.Text; }
        }

        private void selectDocumentDirectoryPathButton_Click(object sender, EventArgs e) {
            if (sender is Button button) { SelectDocumentDirectoryPath(); }
        }

        private void indexDocumentDirectoryCheckBox_CheckStateChanged(object sender, EventArgs e) {
            if (sender is CheckBox checkBox) { ViewModel.Index = checkBox.CheckState == CheckState.Checked; }
        }

        private void watchDocumentDirectoryCheckBox_CheckStateChanged(object sender, EventArgs e) {
            if (sender is CheckBox checkBox) { ViewModel.Watch = checkBox.CheckState == CheckState.Checked; }
        }

        private void saveAndCloseButton_Click(object sender, EventArgs e) {
            if (sender is Button button) {
                if (!ValidateViewModel()) { return; }

                var actions = new Action<CancellationToken, IProgress<ProgressInfo>>[] {
                    (token, progress) => SaveDocumentDirectory(token, progress),
                    (token, progress) => SaveDocumentCollection(token, progress),
                    (token, progress) => CleanDocumentDirectory(token, progress),
                    (token, progress) => IndexDocumentDirectory(token, progress),
                    (token, progress) => WatchDocumentDirectory(token, progress)
                };

                ProgressViewer.Display(_cancellationTokenIssuer, log: Log, actions: actions);

                DialogResult = DialogResult.OK;
            }
        }

        #endregion Event Handlers
    }
}