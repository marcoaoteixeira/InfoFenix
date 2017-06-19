using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using InfoFenix.Client.Code;
using InfoFenix.Core;
using InfoFenix.Core.Logging;
using InfoFenix.Core.PubSub;
using Resource = InfoFenix.Client.Properties.Resources;

namespace InfoFenix.Client {

    public partial class SplashScreenForm : Form {

        #region Private Read-Only Fields

        private readonly IPublisherSubscriber _publisherSubscriber;

        #endregion Private Read-Only Fields

        #region Private Fields

        private ISubscription<ProgressiveTaskStartNotification> _progressiveTaskStartSubscription;
        private ISubscription<ProgressiveTaskPerformStepNotification> _progressiveTaskPerformStepSubscription;
        private ISubscription<ProgressiveTaskCompleteNotification> _progressiveTaskCompleteSubscription;

        #endregion Private Fields

        #region Public Properties

        private ILogger _log;

        public ILogger Log {
            get { return _log ?? NullLogger.Instance; }
            set { _log = value ?? NullLogger.Instance; }
        }

        #endregion Public Properties

        #region Public Constructors

        public SplashScreenForm(IPublisherSubscriber publisherSubscriber) {
            Prevent.ParameterNull(publisherSubscriber, nameof(publisherSubscriber));

            _publisherSubscriber = publisherSubscriber;

            InitializeComponent();
        }

        #endregion Public Constructors

        #region Public Static Methods

        public static void Splash(Func<SplashScreenForm> factory, Action action) {
            using (var form = factory()) {
                Task
                    .Run(action)
                    .ContinueWith(continuationTask => form.DialogResult = DialogResult.OK);

                form.ShowDialog();
            }
        }

        #endregion Public Static Methods

        #region Private Methods

        private void Initialize() {
            messageLabel.Text = string.Empty;
            progressLabel.Text = string.Empty;
            versionLabel.Text = string.Format(Resource.SplashScreen_Version, EntryPoint.ApplicationVersion.Major, EntryPoint.ApplicationVersion.Minor, EntryPoint.ApplicationVersion.Revision);
        }

        private void SubscribeForNotifications() {
            _progressiveTaskStartSubscription = _publisherSubscriber.Subscribe<ProgressiveTaskStartNotification>(ProgressiveTaskStartHandler);
            _progressiveTaskPerformStepSubscription = _publisherSubscriber.Subscribe<ProgressiveTaskPerformStepNotification>(ProgressiveTaskPerformStepHandler);
            _progressiveTaskCompleteSubscription = _publisherSubscriber.Subscribe<ProgressiveTaskCompleteNotification>(ProgressiveTaskCompleteHandler);
        }

        private void UnsubscribeForNotifications() {
            _publisherSubscriber.Unsubscribe(_progressiveTaskStartSubscription);
            _publisherSubscriber.Unsubscribe(_progressiveTaskPerformStepSubscription);
            _publisherSubscriber.Unsubscribe(_progressiveTaskCompleteSubscription);
        }

        private void ProgressiveTaskStartHandler(ProgressiveTaskStartNotification message) {
            messageLabel.SafeCall(() => messageLabel.Text = message.Arguments.Message);
            progressLabel.SafeCall(() => progressLabel.Text = string.Format(Resource.SplashScreen_ActualStep_TotalSteps, message.Arguments.ActualStep, message.Arguments.TotalSteps));
        }

        private void ProgressiveTaskPerformStepHandler(ProgressiveTaskPerformStepNotification message) {
            messageLabel.SafeCall(() => messageLabel.Text = message.Arguments.Message);
            progressLabel.SafeCall(() => progressLabel.Text = string.Format(Resource.SplashScreen_ActualStep_TotalSteps, message.Arguments.ActualStep, message.Arguments.TotalSteps));
        }

        private void ProgressiveTaskCompleteHandler(ProgressiveTaskCompleteNotification message) {
            messageLabel.SafeCall(() => messageLabel.Text = message.Arguments.Message);
            progressLabel.SafeCall(() => progressLabel.Text = string.Format(Resource.SplashScreen_ActualStep_TotalSteps, message.Arguments.ActualStep, message.Arguments.TotalSteps));
        }

        #endregion Private Methods

        #region Event Handlers

        private void SplashScreenForm_Load(object sender, EventArgs e) {
            Initialize();
            SubscribeForNotifications();
        }

        private void SplashScreenForm_FormClosing(object sender, FormClosingEventArgs e) {
            UnsubscribeForNotifications();
        }

        #endregion Event Handlers
    }
}