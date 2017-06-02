using System;
using System.Windows.Forms;
using InfoFenix.Client.Code;
using InfoFenix.Core;
using InfoFenix.Core.PubSub;

namespace InfoFenix.Client.Views {

    public partial class ProgressForm : Form {

        #region Public Events

        public event Action<object, EventArgs> CancelProgressiveTask;

        public event Action<object, EventArgs> CompleteProgressiveTask;

        #endregion Public Events

        #region Private Read-Only Fields

        private readonly IPublisherSubscriber _publisherSubscriber;

        #endregion Private Read-Only Fields

        #region Private Fields

        private ISubscription<ProgressiveTaskStartNotification> _progressiveTaskStartSubscription;
        private ISubscription<ProgressiveTaskPerformStepNotification> _progressiveTaskPerformStepSubscription;
        private ISubscription<ProgressiveTaskCompleteNotification> _progressiveTaskCompleteSubscription;

        #endregion Private Fields

        #region Public Constructors

        public ProgressForm(IPublisherSubscriber publisherSubscriber) {
            Prevent.ParameterNull(publisherSubscriber, nameof(publisherSubscriber));

            _publisherSubscriber = publisherSubscriber;

            InitializeComponent();
        }

        #endregion Public Constructors

        #region Public Methods

        public void Initialize() {
            titleLabel.Text = string.Empty;
            messageLabel.Text = string.Empty;
            mainProgressBar.Step = 1;
            mainProgressBar.Minimum = 0;
            mainProgressBar.Maximum = 100;
            progressLabel.Text = string.Empty;
        }

        #endregion Public Methods

        #region Private Methods

        private void SubscribeForNotification() {
            _progressiveTaskStartSubscription = _publisherSubscriber.Subscribe<ProgressiveTaskStartNotification>(ProgressiveTaskStartHandler);
            _progressiveTaskPerformStepSubscription = _publisherSubscriber.Subscribe<ProgressiveTaskPerformStepNotification>(ProgressiveTaskPerformStepHandler);
            _progressiveTaskCompleteSubscription = _publisherSubscriber.Subscribe<ProgressiveTaskCompleteNotification>(ProgressiveTaskCompleteHandler);
        }

        private void UnsubscribeFromNotification() {
            _publisherSubscriber.Unsubscribe(_progressiveTaskStartSubscription);
            _publisherSubscriber.Unsubscribe(_progressiveTaskPerformStepSubscription);
            _publisherSubscriber.Unsubscribe(_progressiveTaskCompleteSubscription);
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

            OnCompleteProgressiveTask(this, EventArgs.Empty);
        }

        private void OnCancelProgressiveTask(object sender, EventArgs e) {
            CancelProgressiveTask?.Invoke(sender, e);
        }

        private void OnCompleteProgressiveTask(object sender, EventArgs e) {
            CompleteProgressiveTask?.Invoke(sender, e);
        }

        #endregion Private Methods

        #region Event Handlers

        private void ProgressForm_Load(object sender, EventArgs e) {
            SubscribeForNotification();
        }

        private void ProgressForm_FormClosing(object sender, FormClosingEventArgs e) {
            UnsubscribeFromNotification();
        }

        private void cancelButton_Click(object sender, EventArgs e) {
            OnCancelProgressiveTask(this, EventArgs.Empty);
        }

        #endregion Event Handlers
    }
}