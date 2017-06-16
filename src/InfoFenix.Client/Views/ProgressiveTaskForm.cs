using System;
using System.Drawing;
using System.Windows.Forms;
using InfoFenix.Client.Code;
using InfoFenix.Core;
using InfoFenix.Core.PubSub;

namespace InfoFenix.Client.Views {

    public partial class ProgressiveTaskForm : Form {

        #region Public Events

        public event Action<object, ProgressiveTaskCancelEventArgs> CancelProgressiveTask;

        public event Action<object, ProgressiveTaskCompleteEventArgs> CompleteProgressiveTask;

        #endregion Public Events

        #region Private Read-Only Fields

        private readonly IPublisherSubscriber _publisherSubscriber;

        #endregion Private Read-Only Fields

        #region Private Fields

        private bool _canCancel;

        private ISubscription<ProgressiveTaskStartNotification> _progressiveTaskStartSubscription;
        private ISubscription<ProgressiveTaskPerformStepNotification> _progressiveTaskPerformStepSubscription;
        private ISubscription<ProgressiveTaskCompleteNotification> _progressiveTaskCompleteSubscription;
        private ISubscription<ProgressiveTaskErrorNotification> _progressiveTaskCriticalErrorSubscription;
        private ISubscription<ProgressiveTaskCancelNotification> _progressiveTaskCancelSubscription;

        #endregion Private Fields

        #region Public Constructors

        public ProgressiveTaskForm(IPublisherSubscriber publisherSubscriber) {
            Prevent.ParameterNull(publisherSubscriber, nameof(publisherSubscriber));

            _publisherSubscriber = publisherSubscriber;

            InitializeComponent();
        }

        #endregion Public Constructors

        #region Public Methods

        public void Initialize(bool canCancel = true) {
            _canCancel = canCancel;

            titleLabel.Text = string.Empty;
            messageLabel.Text = string.Empty;
            mainProgressBar.Step = 1;
            mainProgressBar.Minimum = 0;
            mainProgressBar.Maximum = 100;
            progressLabel.Text = string.Empty;
            
            cancelButton.Visible = _canCancel;
            okButton.Visible = !_canCancel;
            okButton.Enabled = !_canCancel;

            // Fix OK Button position
            okButton.Location = new Point(cancelButton.Location.X, cancelButton.Location.Y);
            if (_canCancel) { okButton.SendToBack(); } else { okButton.BringToFront(); }
        }

        private void Finish() {
            cancelButton.SafeCall(() => cancelButton.Visible = false);
            okButton.SafeCall(() => {
                okButton.Visible = true;
                okButton.Enabled = true;
            });
            
            // Fix OK Button position
            var newPoint = new Point(cancelButton.Location.X, cancelButton.Location.Y);
            okButton.SafeCall(() => okButton.Location = newPoint);
            okButton.SafeCall(() => okButton.BringToFront());
        }

        #endregion Public Methods

        #region Private Methods

        private void SubscribeForNotification() {
            _progressiveTaskStartSubscription = _publisherSubscriber.Subscribe<ProgressiveTaskStartNotification>(ProgressiveTaskStartHandler);
            _progressiveTaskPerformStepSubscription = _publisherSubscriber.Subscribe<ProgressiveTaskPerformStepNotification>(ProgressiveTaskPerformStepHandler);
            _progressiveTaskCompleteSubscription = _publisherSubscriber.Subscribe<ProgressiveTaskCompleteNotification>(ProgressiveTaskCompleteHandler);
            _progressiveTaskCriticalErrorSubscription = _publisherSubscriber.Subscribe<ProgressiveTaskErrorNotification>(ProgressiveTaskCriticalErrorHandler);
            _progressiveTaskCancelSubscription = _publisherSubscriber.Subscribe<ProgressiveTaskCancelNotification>(ProgressiveTaskCancelHandler);
        }

        private void UnsubscribeFromNotification() {
            _publisherSubscriber.Unsubscribe(_progressiveTaskStartSubscription);
            _publisherSubscriber.Unsubscribe(_progressiveTaskPerformStepSubscription);
            _publisherSubscriber.Unsubscribe(_progressiveTaskCompleteSubscription);
            _publisherSubscriber.Unsubscribe(_progressiveTaskCriticalErrorSubscription);
            _publisherSubscriber.Unsubscribe(_progressiveTaskCancelSubscription);
        }

        private void ProgressiveTaskStartHandler(ProgressiveTaskStartNotification message) {
            titleLabel.SafeCall(() => titleLabel.Text = message.Title);
            messageLabel.SafeCall(() => messageLabel.Text = message.Message);
            progressLabel.SafeCall(() => progressLabel.Text = $"1 de {message.TotalSteps}");
            mainProgressBar.SafeCall(() => {
                mainProgressBar.Step = 1;
                mainProgressBar.Minimum = 1;
                mainProgressBar.Maximum = message.TotalSteps;
            });
        }

        private void ProgressiveTaskPerformStepHandler(ProgressiveTaskPerformStepNotification message) {
            titleLabel.SafeCall(() => titleLabel.Text = message.Title);
            messageLabel.SafeCall(() => messageLabel.Text = message.Message);
            progressLabel.SafeCall(() => progressLabel.Text = $"{message.ActualStep} de {message.TotalSteps}");
            mainProgressBar.SafeCall(() => mainProgressBar.PerformStep());
        }

        private void ProgressiveTaskCompleteHandler(ProgressiveTaskCompleteNotification message) {
            titleLabel.SafeCall(() => titleLabel.Text = message.Title);
            messageLabel.SafeCall(() => messageLabel.Text = message.Message);
            progressLabel.SafeCall(() => progressLabel.Text = $"{message.TotalSteps} de {message.TotalSteps}");

            OnCompleteProgressiveTask(this, new ProgressiveTaskCompleteEventArgs());
        }

        private void ProgressiveTaskCriticalErrorHandler(ProgressiveTaskErrorNotification message) {
            titleLabel.SafeCall(() => titleLabel.Text = message.Title);
            messageLabel.SafeCall(() => messageLabel.Text = message.Message);
            progressLabel.SafeCall(() => progressLabel.Text = $"{message.TotalSteps} de {message.TotalSteps}");

            OnCancelProgressiveTask(this, new ProgressiveTaskCancelEventArgs { Force = true });
        }

        private void ProgressiveTaskCancelHandler(ProgressiveTaskCancelNotification message) {
            titleLabel.SafeCall(() => titleLabel.Text = message.Title);
            messageLabel.SafeCall(() => messageLabel.Text = message.Message);
            progressLabel.SafeCall(() => progressLabel.Text = $"{message.TotalSteps} de {message.TotalSteps}");

            OnCancelProgressiveTask(this, new ProgressiveTaskCancelEventArgs { Force = true });
        }

        private void OnCancelProgressiveTask(object sender, ProgressiveTaskCancelEventArgs e) {
            CancelProgressiveTask?.Invoke(sender, e);
            Finish();
        }

        private void OnCompleteProgressiveTask(object sender, ProgressiveTaskCompleteEventArgs e) {
            CompleteProgressiveTask?.Invoke(sender, e);
            Finish();
        }

        #endregion Private Methods

        #region Event Handlers

        private void ProgressiveTaskForm_Load(object sender, EventArgs e) {
            SubscribeForNotification();
        }

        private void ProgressiveTaskForm_FormClosing(object sender, FormClosingEventArgs e) {
            UnsubscribeFromNotification();
        }

        private void cancelButton_Click(object sender, EventArgs e) {
            OnCancelProgressiveTask(this, new ProgressiveTaskCancelEventArgs { Force = false });
        }

        private void okButton_Click(object sender, EventArgs e) {
            DialogResult = DialogResult.OK;
        }

        #endregion Event Handlers
    }
}