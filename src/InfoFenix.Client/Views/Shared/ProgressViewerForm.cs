using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using InfoFenix.Core;
using InfoFenix.Core.Cqrs;
using Resource = InfoFenix.Client.Properties.Resources;

namespace InfoFenix.Client.Views.Shared {

    public partial class ProgressViewerForm : LayoutForm {

        #region Public Constants

        public const string PROGRESS_VIEWER_CANCELLATION_TOKEN_KEY = "PROGRESS::VIEWER::CANCELLATION::TOKEN";

        #endregion Public Constants

        #region Private Read-Only Fields

        private readonly CancellationTokenIssuer _cancellationTokenIssuer;

        #endregion Private Read-Only Fields

        #region Private Fields
        
        private BackgroundWorker _backgroundWorker;

        #endregion Private Fields

        #region Private Properties

        private BackgroundWorkerArguments Arguments { get; } = new BackgroundWorkerArguments();

        #endregion

        #region Public Constructors

        public ProgressViewerForm(CancellationTokenIssuer cancellationTokenIssuer) {
            Prevent.ParameterNull(cancellationTokenIssuer, nameof(cancellationTokenIssuer));

            _cancellationTokenIssuer = cancellationTokenIssuer;

            InitializeComponent();
        }

        #endregion Public Constructors

        #region Public Methods

        public void Run(params Action<CancellationToken, IProgress<ProgressInfo>>[] actions) {
            using (_backgroundWorker = new BackgroundWorker()) {
                _backgroundWorker.DoWork += DoWorkHandler;
                _backgroundWorker.ProgressChanged += ProgressChangeHandler;
                _backgroundWorker.RunWorkerCompleted += RunWorkerCompleteHandler;

                _backgroundWorker.WorkerReportsProgress = true;
                _backgroundWorker.WorkerSupportsCancellation = true;

                Arguments.Actions = actions;
                Arguments.Token = _cancellationTokenIssuer.Get(PROGRESS_VIEWER_CANCELLATION_TOKEN_KEY);
                Arguments.Progress = new Progress<ProgressInfo>(ProgressNotificationHandler);
                
                _backgroundWorker.RunWorkerAsync(Arguments);
            }
        }
        
        #endregion Public Methods

        #region Private Methods

        private void InitializeDefaultState() {
            progressTitleLabel.Text = string.Empty;
            progressMessageLabel.Text = string.Empty;
            mainProgressBar.Step = 1;
            mainProgressBar.Minimum = 0;
            mainProgressBar.Maximum = 100;
            closeButton.Text = Resource.ProgressViewForm_ActionButton_Close;
        }

        private void ProgressNotificationHandler(ProgressInfo info) {
            switch (info.State) {
                case ProgressState.Start:
                    StartNotificationHandler(info);
                    break;

                case ProgressState.PerformStep:
                    PerformStepNotificationHandler(info);
                    break;

                case ProgressState.Complete:
                    CompleteNotificationHandler(info);
                    break;

                case ProgressState.Cancel:
                    CancelNotificationHandler(info);
                    break;

                case ProgressState.Error:
                    ErrorNotificationHandler(info);
                    break;
            }
        }

        private void StartNotificationHandler(ProgressInfo info) {
            progressTitleLabel.Text = info.Title;
            progressMessageLabel.Text = info.Message;
            mainProgressBar.Value = 0;
            mainProgressBar.Minimum = info.ActualStep;
            mainProgressBar.Maximum = info.TotalSteps;
            closeButton.Text = Resource.ProgressViewForm_ActionButton_Cancel;
        }

        private void PerformStepNotificationHandler(ProgressInfo info) {
            progressMessageLabel.Text = info.Message;
            mainProgressBar.PerformStep();
        }

        private void CompleteNotificationHandler(ProgressInfo info) {
            progressMessageLabel.Text = info.Message;
        }

        private void CancelNotificationHandler(ProgressInfo info) {
            progressMessageLabel.Text = info.Message;
        }

        private void ErrorNotificationHandler(ProgressInfo info) {
            progressMessageLabel.Text = info.Message;

            _cancellationTokenIssuer.Cancel(PROGRESS_VIEWER_CANCELLATION_TOKEN_KEY);
        }

        private bool ConfirmTaskCancellation() {
            if (!_backgroundWorker.IsBusy) { return true; }

            var response = MessageBox.Show(Resource.ProgressViewForm_Cancellation_Message,
                Resource.ProgressViewForm_Cancellation_Title,
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (response == DialogResult.No) { return false; }

            return _cancellationTokenIssuer.Cancel(PROGRESS_VIEWER_CANCELLATION_TOKEN_KEY);
        }

        #endregion Private Methods

        #region Event Handlers

        private void ProgressViewerForm_Load(object sender, EventArgs e) {
            InitializeDefaultState();
        }

        private void ProgressViewerForm_FormClosing(object sender, FormClosingEventArgs e) {
            if (!ConfirmTaskCancellation()) {
                e.Cancel = true;
                return;
            }
        }

        private void closeButton_Click(object sender, EventArgs e) {
            if (!ConfirmTaskCancellation()) {
                return;
            }

            DialogResult = DialogResult.OK;
        }

        private void DoWorkHandler(object sender, DoWorkEventArgs e) {
            var worker = sender as BackgroundWorker;
            try {
                var arguments = e.Argument as BackgroundWorkerArguments;
                foreach (var action in arguments.Actions) {
                    action(arguments.Token, arguments.Progress);
                }
            } catch (AggregateException ex) {
                ex.Handle(_ => {
                    if (_ is TaskCanceledException) { return true; } /* Just ignore, task canceled for reasons */
                    if (_ is Exception) { return true; } /* Should notify user? Nahh... */

                    return false;
                });
            } catch (Exception) { throw; }
        }

        private void ProgressChangeHandler(object sender, ProgressChangedEventArgs e) {
        }

        private void RunWorkerCompleteHandler(object sender, RunWorkerCompletedEventArgs e) {
            closeButton.Text = Resource.ProgressViewForm_ActionButton_Close;
        }

        #endregion Event Handlers

        #region Private Inner Classes

        private class BackgroundWorkerArguments {
            #region Public Properties

            public CancellationToken Token { get; set; }
            public IEnumerable<Action<CancellationToken, IProgress<ProgressInfo>>> Actions { get; set; } = Enumerable.Empty<Action<CancellationToken, IProgress<ProgressInfo>>>();
            public IProgress<ProgressInfo> Progress { get; set; }

            #endregion
        }

        #endregion
    }
}