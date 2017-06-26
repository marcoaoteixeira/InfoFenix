using System;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using InfoFenix.Client.Code;
using InfoFenix.Client.Code.Helpers;
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
        private readonly ProgressiveTaskExecutor _progressiveTaskExecutor;

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

        public DocumentDirectoryForm(CancellationTokenIssuer cancellationTokenIssuer, IMediator mediator, IPublisherSubscriber publisherSubscriber, ProgressiveTaskExecutor progressiveTaskExecutor) {
            Prevent.ParameterNull(cancellationTokenIssuer, nameof(cancellationTokenIssuer));
            Prevent.ParameterNull(mediator, nameof(mediator));
            Prevent.ParameterNull(publisherSubscriber, nameof(publisherSubscriber));
            Prevent.ParameterNull(progressiveTaskExecutor, nameof(progressiveTaskExecutor));

            _cancellationTokenIssuer = cancellationTokenIssuer;
            _mediator = mediator;
            _publisherSubscriber = publisherSubscriber;
            _progressiveTaskExecutor = progressiveTaskExecutor;

            InitializeComponent();
        }

        #endregion Public Constructors

        #region Private Methods

        private void Initialize() {
            ViewModel = ViewModel ?? new DocumentDirectoryDto {
                Code = Guid.NewGuid().ToString("N")
            };
        }

        private bool ValidateViewModel() {
            var errors = Validator.Validate(ViewModel);
            if (errors.IsValid) { return true; }
            MessageBox.Show(errors[0].Message, Resource.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }

        private void SaveDocumentDirectory(CancellationToken cancellationToken) {
            _mediator
                .CommandAsync(new SaveDocumentDirectoryCommand {
                    DocumentDirectory = ViewModel
                }, cancellationToken)
                .WaitForResult();
        }

        private void SaveDocumentCollection(CancellationToken cancellationToken) {
            ViewModel.Documents = _mediator
                .Query(new ListDocumentsDifferenceBetweenDiskDatabaseQuery {
                    DocumentDirectoryID = ViewModel.DocumentDirectoryID
                })
                .ToList();

            _mediator
                .CommandAsync(new SaveDocumentCollectionCommand {
                    DocumentDirectoryID = ViewModel.DocumentDirectoryID,
                    Documents = ViewModel.Documents
                }, cancellationToken)
                .WaitForResult();
        }

        private void CleanDocumentDirectory(CancellationToken cancellationToken) {
            _mediator
                .CommandAsync(new CleanDocumentDirectoryCommand {
                    DocumentDirectory = ViewModel
                }, cancellationToken)
                .WaitForResult();
        }

        private void IndexDocumentDirectory(CancellationToken cancellationToken) {
            if (!ViewModel.Index) { return; }

            _mediator
                .CommandAsync(new IndexDocumentCollectionCommand {
                    BatchSize = 128,
                    Documents = ViewModel.Documents.Select(DocumentIndexDto.Map).ToList(),
                    IndexName = ViewModel.Code,
                }, cancellationToken)
                .WaitForResult();
        }

        private void WatchDocumentDirectory(CancellationToken cancellationToken) {
            ICommand command;
            if (ViewModel.Watch) { command = new StartWatchDocumentDirectoryCommand { DocumentDirectory = ViewModel }; }
            else { command = new StopWatchDocumentDirectoryCommand { DocumentDirectory = ViewModel }; }

            _mediator
                .CommandAsync(command, cancellationToken)
                .WaitForResult();
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
            Initialize();
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

                var actions = new Action<CancellationToken>[] {
                    (token) => SaveDocumentDirectory(token),
                    (token) => SaveDocumentCollection(token),
                    (token) => CleanDocumentDirectory(token),
                    (token) => IndexDocumentDirectory(token),
                    (token) => WatchDocumentDirectory(token)
                };

                ProgressiveTaskExecutor.Instance.Run(cancellationTokenIssuer: _cancellationTokenIssuer,
                    publisherSubscriber: _publisherSubscriber,
                    actions: actions);

                DialogResult = DialogResult.OK;
            }
        }

        #endregion Event Handlers
    }
}