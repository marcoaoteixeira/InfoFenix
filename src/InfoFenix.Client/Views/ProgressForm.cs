using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using InfoFenix.Client.Code;
using InfoFenix.Core;
using InfoFenix.Core.Cqrs;
using InfoFenix.Core.Logging;
using InfoFenix.Core.PubSub;

namespace InfoFenix.Client.Views {

    public partial class ProgressForm : Form {

        #region Private Static Read-Only Fields

        private static readonly string CancellationTokenKey = $"{nameof(ProgressForm)}::EXECUTING_TASK";

        #endregion Private Static Read-Only Fields

        #region Private Read-Only Fields

        private readonly CancellationTokenIssuer _cancellationTokenIssuer;
        private readonly ICqrsDispatcher _cqrsDispatcher;
        private readonly IPublisherSubscriber _publisherSubscriber;

        #endregion Private Read-Only Fields

        #region Private Fields

        private ISubscription<StartingProgressiveTaskNotification> _startingProgressiveTaskNotificationSubscription;
        private ISubscription<ProgressiveTaskCompletedNotification> _progressiveTaskCompletedNotificationSubscription;
        private ISubscription<ProgressiveTaskPerformStepNotification> _progressiveTaskPerformStepNotificationSubscription;
        private bool _detailsToggled;

        #endregion Private Fields

        #region Private Volatile Fields

        private volatile bool _working;

        #endregion Private Volatile Fields

        #region Public Properties

        private ILogger _log;

        public ILogger Log {
            get { return _log ?? NullLogger.Instance; }
            set { _log = value ?? NullLogger.Instance; }
        }

        public ICommand Task { get; set; }

        #endregion Public Properties

        #region Public Constructors

        public ProgressForm(CancellationTokenIssuer cancellationTokenIssuer, ICqrsDispatcher cqrsDispatcher, IPublisherSubscriber publisherSubscriber) {
            Prevent.ParameterNull(cancellationTokenIssuer, nameof(cancellationTokenIssuer));
            Prevent.ParameterNull(cqrsDispatcher, nameof(cqrsDispatcher));
            Prevent.ParameterNull(publisherSubscriber, nameof(publisherSubscriber));

            _cancellationTokenIssuer = cancellationTokenIssuer;
            _cqrsDispatcher = cqrsDispatcher;
            _publisherSubscriber = publisherSubscriber;

            SubscribeForNotification();
            InitializeComponent();

            // Shortcut...
            RichTextBoxAppender.SetRichTextBox(progressLogRichTextBox, nameof(RichTextBoxAppender));
        }

        #endregion Public Constructors

        #region Private Methods

        private void SubscribeForNotification() {
            _startingProgressiveTaskNotificationSubscription = _publisherSubscriber.Subscribe<StartingProgressiveTaskNotification>(StartingProgressiveTaskHandler);
            _progressiveTaskCompletedNotificationSubscription = _publisherSubscriber.Subscribe<ProgressiveTaskCompletedNotification>(ProgressiveTaskCompletedHandler);
            _progressiveTaskPerformStepNotificationSubscription = _publisherSubscriber.Subscribe<ProgressiveTaskPerformStepNotification>(ProgressiveTaskPerformStepHandler);
        }

        private void UnsubscribeFromNotification() {
            _publisherSubscriber.Unsubscribe(_startingProgressiveTaskNotificationSubscription);
            _publisherSubscriber.Unsubscribe(_progressiveTaskCompletedNotificationSubscription);
            _publisherSubscriber.Unsubscribe(_progressiveTaskPerformStepNotificationSubscription);
        }

        private void StartingProgressiveTaskHandler(StartingProgressiveTaskNotification message) {
            titleLabel.SafeCall(() => titleLabel.Text = message.Title);
            messageLabel.SafeCall(() => messageLabel.Text = message.Message);
            progressLabel.SafeCall(() => progressLabel.Text = $"1 de {message.MaximunValue}");
            mainProgressBar.SafeCall(() => {
                mainProgressBar.Minimum = message.MinimunValue;
                mainProgressBar.Maximum = message.MaximunValue;
            });
        }

        private void ProgressiveTaskCompletedHandler(ProgressiveTaskCompletedNotification message) {
            titleLabel.SafeCall(() => titleLabel.Text = message.Title);
            messageLabel.SafeCall(() => messageLabel.Text = message.Message);
            progressLabel.SafeCall(() => progressLabel.Text = $"{message.TotalSteps} de {message.TotalSteps}");
            _working = false;
        }

        private void ProgressiveTaskPerformStepHandler(ProgressiveTaskPerformStepNotification message) {
            titleLabel.SafeCall(() => titleLabel.Text = message.Title);
            messageLabel.SafeCall(() => messageLabel.Text = message.Message);
            progressLabel.SafeCall(() => progressLabel.Text = $"{message.ActualStep} de {message.StepsToCompletation}");
            mainProgressBar.SafeCall(() => mainProgressBar.PerformStep());
        }

        private void Initialize() {
            Height = 210; /* Hides the log rich text box */

            titleLabel.Text = string.Empty;
            messageLabel.Text = string.Empty;
            mainProgressBar.Minimum = 0;
            mainProgressBar.Maximum = 100;
            progressLabel.Text = string.Empty;
            _working = false;
        }

        private void StartTask() {
            if (Task == null) { return; }
            _working = true;
            _cqrsDispatcher
                .CommandAsync(Task, _cancellationTokenIssuer.Get(CancellationTokenKey))
                .ContinueWith(CompleteTask);
        }

        private void StopRunningTask() {
            _cancellationTokenIssuer.Cancel(CancellationTokenKey);
            MessageBox.Show("A tarefa foi cancelada.", "Cancelado", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void CompleteTask(Task task) {
            _working = false;
            _cancellationTokenIssuer.MarkAsComplete(CancellationTokenKey);
            if (task.Exception != null) {
                Log.Error(task.Exception, task.Exception.Message);
                MessageBox.Show($"Ocorreu um erro na execução da tarefa: {task.Exception.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } else {
                MessageBox.Show("A tarefa concluída com sucesso.", "Fim", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            this.SafeCall(() => Close());
        }

        #endregion Private Methods

        #region Event Handlers

        private void ProgressForm_Load(object sender, EventArgs e) {
            Initialize();
            StartTask();
        }

        private void ProgressForm_FormClosing(object sender, FormClosingEventArgs e) {
            if (_working) {
                var result = MessageBox.Show("Deseja realmente parar a tarefa?", "Parar tarefa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result != DialogResult.Yes) { e.Cancel = true; return; } else { StopRunningTask(); }
            }
            UnsubscribeFromNotification();
        }

        private void stopButton_Click(object sender, EventArgs e) {
            Close();
        }

        private void toggleDetailsLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            var linkLabel = sender as LinkLabel;
            if (linkLabel == null) { return; }

            if (!_detailsToggled) {
                linkLabel.Text = "Menos detalhes";
                Height += 200;
                System.Threading.Tasks.Task.Run(() => {
                    System.Threading.Thread.Sleep(500);
                    for (int i = 0; i < 10; i++) {
                        Log.Debug("Debug Line");
                        Log.Error("Error Line");
                        Log.Fatal("Fatal Line");
                        Log.Information("Information Line");
                        Log.Warning("Warning Line");
                    }
                });
            } else {
                linkLabel.Text = "Mais detalhes";
                Height -= 200;
            }

            StartPosition = FormStartPosition.CenterScreen;

            _detailsToggled = !_detailsToggled;
        }

        #endregion Event Handlers
    }
}