﻿using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using InfoFenix.Application.Code;
using InfoFenix;
using InfoFenix.Logging;
using InfoFenix.PubSub;
using InfoFenix.Resources;

namespace InfoFenix.Application.Views.Home {

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
                    .ContinueWith(form.Continuation, form);

                form.ShowDialog();
            }
        }

        #endregion Public Static Methods

        #region Private Methods

        private void Initialize() {
            messageLabel.Text = string.Empty;
            progressLabel.Text = string.Empty;
            versionLabel.Text = string.Format(Strings.Display_Version, EntryPoint.ApplicationVersion.Major, EntryPoint.ApplicationVersion.Minor, EntryPoint.ApplicationVersion.Revision);
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
            messageLabel.SafeInvoke(_ => _.Text = message.Arguments.Message);
            progressLabel.SafeInvoke(_ => _.Text = string.Format(Strings.SplashScreenForm_ActualStep_TotalSteps, message.Arguments.ActualStep, message.Arguments.TotalSteps));
        }

        private void ProgressiveTaskPerformStepHandler(ProgressiveTaskPerformStepNotification message) {
            messageLabel.SafeInvoke(_ => _.Text = message.Arguments.Message);
            progressLabel.SafeInvoke(_ => _.Text = string.Format(Strings.SplashScreenForm_ActualStep_TotalSteps, message.Arguments.ActualStep, message.Arguments.TotalSteps));
        }

        private void ProgressiveTaskCompleteHandler(ProgressiveTaskCompleteNotification message) {
            messageLabel.SafeInvoke(_ => _.Text = message.Arguments.Message);
            progressLabel.SafeInvoke(_ => _.Text = string.Format(Strings.SplashScreenForm_ActualStep_TotalSteps, message.Arguments.ActualStep, message.Arguments.TotalSteps));
        }

        private void Continuation(Task continuation, object state) {
            var form = state as Form;
            if (form != null) {
                form.SafeInvoke(_ => _.DialogResult = DialogResult.OK);
            }
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