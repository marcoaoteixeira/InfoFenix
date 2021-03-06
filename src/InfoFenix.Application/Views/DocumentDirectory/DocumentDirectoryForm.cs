﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using InfoFenix.Application.Code;
using InfoFenix.Application.Code.Helpers;
using InfoFenix.CQRS;
using InfoFenix.Domains.Commands;
using InfoFenix.Infrastructure;
using InfoFenix.Logging;
using InfoFenix.PubSub;
using InfoFenix.Resources;

namespace InfoFenix.Application.Views.DocumentDirectory {

    public partial class DocumentDirectoryForm : Form {

        #region Public Constants

        public const int MIN_POSITION = 0;
        public const int MAX_POSITION = 999999;

        #endregion Public Constants

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

        public Entities.DocumentDirectory ViewModel { get; set; }

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
            ViewModel = ViewModel ?? new Entities.DocumentDirectory {
                Code = Guid.NewGuid().ToString("N")
            };
            labelTextBox.Text = ViewModel.Label;
            pathTextBox.Text = ViewModel.Path;

            positionNumericUpDown.Minimum = MIN_POSITION;
            positionNumericUpDown.Maximum = MAX_POSITION;
            positionNumericUpDown.Value = ViewModel.Position;
        }

        private bool ValidateViewModel() {
            var errors = Validator.Validate(ViewModel);
            if (errors.IsValid) { return true; }
            MessageBox.Show(errors[0].Message, Strings.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }

        private void SaveDocumentDirectory(CancellationToken cancellationToken, IProgress<ProgressInfo> progress) {
            var command = new SaveDocumentDirectoryCommand {
                DocumentDirectoryID = ViewModel.DocumentDirectoryID,
                Code = ViewModel.Code,
                Label = ViewModel.Label,
                Path = ViewModel.Path,
                Position = ViewModel.Position
            };

            _mediator.CommandAsync(command, cancellationToken, progress).Wait();

            ViewModel.DocumentDirectoryID = command.DocumentDirectoryID;
        }

        private void SaveDocumentDirectoryDocuments(CancellationToken cancellationToken, IProgress<ProgressInfo> progress) {
            _mediator
                .CommandAsync(new SaveDocumentDirectoryDocumentsCommand {
                    DocumentDirectoryID = ViewModel.DocumentDirectoryID
                }, cancellationToken, progress).Wait();
        }

        private void CleanDocumentDirectory(CancellationToken cancellationToken, IProgress<ProgressInfo> progress) {
            _mediator
                .CommandAsync(new CleanDocumentDirectoryCommand {
                    DocumentDirectoryID = ViewModel.DocumentDirectoryID
                }, cancellationToken, progress).Wait();
        }

        private void IndexDocumentDirectory(CancellationToken cancellationToken, IProgress<ProgressInfo> progress) {
            _mediator
                .CommandAsync(new IndexDocumentDirectoryCommand {
                    DocumentDirectoryID = ViewModel.DocumentDirectoryID,
                    BatchSize = 128
                }, cancellationToken, progress).Wait();
        }

        private void SelectDocumentDirectoryPath() {
            var path = DialogHelper.OpenFolderBrowserDialog(Strings.DocumentDirectoryForm_SelectDocumentDirectoryPath_Message);
            if (!string.IsNullOrWhiteSpace(path)) {
                pathTextBox.Text = path;
            }
        }

        #endregion Private Methods

        #region Event Handlers

        private void DocumentDirectoryForm_Load(object sender, EventArgs e) {
            InitializeDefaultState();
        }

        private void labelTextBox_Leave(object sender, EventArgs e) {
            var textBox = sender as TextBox;
            if (textBox != null) { ViewModel.Label = textBox.Text; }
        }

        private void labelTextBox_TextChanged(object sender, EventArgs e) {
            var textBox = sender as TextBox;
            if (textBox != null) { ViewModel.Label = textBox.Text; }
        }

        private void pathTextBox_Leave(object sender, EventArgs e) {
            var textBox = sender as TextBox;
            if (textBox != null) { ViewModel.Path = textBox.Text; }
        }

        private void pathTextBox_TextChanged(object sender, EventArgs e) {
            var textBox = sender as TextBox;
            if (textBox != null) { ViewModel.Path = textBox.Text; }
        }

        private void selectPathButton_Click(object sender, EventArgs e) {
            var button = sender as Button;
            if (button != null) { SelectDocumentDirectoryPath(); }
        }

        private void positionNumericUpDown_ValueChanged(object sender, EventArgs e) {
            var numericUpDown = sender as NumericUpDown;
            if (numericUpDown != null) { ViewModel.Position = Convert.ToInt32(numericUpDown.Value); }
        }

        private void saveAndCloseButton_Click(object sender, EventArgs e) {
            var button = sender as Button;
            if (button != null) {
                if (!ValidateViewModel()) { return; }

                var actions = new List<Action<CancellationToken, IProgress<ProgressInfo>>> {
                    (token, progress) => SaveDocumentDirectory(token, progress)
                };
                if (saveDocumentDirectoryDocumentsCheckBox.Checked) {
                    actions.Add((token, progress) => SaveDocumentDirectoryDocuments(token, progress));
                    actions.Add((token, progress) => CleanDocumentDirectory(token, progress));
                    actions.Add((token, progress) => IndexDocumentDirectory(token, progress));
                }

                ProgressViewer.Display(_cancellationTokenIssuer, log: Log, actions: actions.ToArray());
                Log.Information($"SAVING DOCUMENT DIRECTORY \"{ViewModel.Label}\".");

                DialogResult = DialogResult.OK;
            }
        }

        #endregion Event Handlers
    }
}