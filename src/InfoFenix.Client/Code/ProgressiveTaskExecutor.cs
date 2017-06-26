using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using InfoFenix.Client.Views.Shared;
using InfoFenix.Core;
using InfoFenix.Core.PubSub;
using Resource = InfoFenix.Client.Properties.Resources;

namespace InfoFenix.Client.Code {

    public sealed class ProgressiveTaskExecutor {

        #region Public Static Read-Only Fields

        public static readonly ProgressiveTaskExecutor Instance = new ProgressiveTaskExecutor();

        #endregion Public Static Read-Only Fields

        #region Private Constructors

        private ProgressiveTaskExecutor() {
        }

        #endregion Private Constructors

        #region Public Methods

        public void Run(CancellationTokenIssuer cancellationTokenIssuer, IPublisherSubscriber publisherSubscriber, params Action<CancellationToken>[] actions) {
            using (var form = new ProgressiveTaskForm(cancellationTokenIssuer, publisherSubscriber)) {
                var cancellationToken = cancellationTokenIssuer.Get(ProgressiveTaskForm.PROGRESSIVE_TASK_CANCELLATION_KEY);
                //var taskScheduler = SynchronizationContext.Current == null
                //    ? TaskScheduler.Default
                //    : TaskScheduler.FromCurrentSynchronizationContext();

                //var parent = new Task(() => actions[0](cancellationToken), cancellationToken);
                //Build(parent, cancellationToken, taskScheduler, 0 /* depth */, actions.Skip(1).ToArray());
                //parent.Start();

                Task.Run(() => {
                    foreach (var action in actions) {
                        action(cancellationToken);
                    }
                });

                form.ShowDialog();
            }
        }

        #endregion Public Static Methods

        #region Private Static Methods

        private static void Build(Task parent, CancellationToken cancellationToken, TaskScheduler taskScheduler, int depth, Action<CancellationToken>[] children) {
            if (children.Length == depth) { return; }

            var continuation = parent.ContinueWith(
                continuationAction: task => children[depth - 1](cancellationToken),
                cancellationToken: cancellationToken,
                continuationOptions: TaskContinuationOptions.NotOnFaulted | TaskContinuationOptions.NotOnCanceled,
                scheduler: taskScheduler);

            Build(continuation, cancellationToken, taskScheduler, ++depth, children);
        }

        #endregion Private Static Methods

        #region Private Inner Classes

        private class ProgressiveTaskForm : LayoutForm {

            #region Public Constants

            public const string PROGRESSIVE_TASK_CANCELLATION_KEY = "PROGRESSIVE_TASK::KEY";

            #endregion Public Constants

            #region Private Read-Only Fields

            private readonly CancellationTokenIssuer _cancellationTokenIssuer;
            private readonly IPublisherSubscriber _publisherSubscriber;

            #endregion Private Read-Only Fields

            #region Private Fields

            /// <summary>
            /// Required designer variable.
            /// </summary>
            private IContainer components = null;

            private Label progressTitleLabel;
            private ProgressBar mainProgressBar;
            private Label progressMessageLabel;

            private ISubscription<ProgressiveTaskStartNotification> _progressiveTaskStartSubscription;
            private ISubscription<ProgressiveTaskPerformStepNotification> _progressiveTaskPerformStepSubscription;
            private ISubscription<ProgressiveTaskCompleteNotification> _progressiveTaskCompleteSubscription;
            private ISubscription<ProgressiveTaskCancelNotification> _progressiveTaskCancelSubscription;
            private ISubscription<ProgressiveTaskErrorNotification> _progressiveTaskErrorSubscription;

            #endregion Private Fields

            #region Private Volatile Fields

            private volatile bool _running;
            private volatile bool _error;

            #endregion Private Volatile Fields

            #region Public Constructors

            public ProgressiveTaskForm(CancellationTokenIssuer cancellationTokenIssuer, IPublisherSubscriber publisherSubscriber) {
                Prevent.ParameterNull(cancellationTokenIssuer, nameof(cancellationTokenIssuer));
                Prevent.ParameterNull(publisherSubscriber, nameof(publisherSubscriber));

                _cancellationTokenIssuer = cancellationTokenIssuer;
                _publisherSubscriber = publisherSubscriber;
                
                InitializeComponent();
            }

            #endregion Public Constructors

            #region Protected Override Methods

            /// <summary>
            /// Clean up any resources being used.
            /// </summary>
            /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
            protected override void Dispose(bool disposing) {
                if (disposing && (components != null)) {
                    components.Dispose();
                }
                base.Dispose(disposing);
            }

            #endregion Protected Override Methods

            #region Private Methods

            private void Initialize() {
                progressTitleLabel.Text = string.Empty;
                progressMessageLabel.Text = string.Empty;
                mainProgressBar.Step = 1;
                mainProgressBar.Minimum = 0;
                mainProgressBar.Maximum = 100;
                closeButton.Text = Resource.ProgressiveTaskForm_ActionButton_Close;
            }

            private void SubscriberForNotifications() {
                _progressiveTaskStartSubscription = _publisherSubscriber.Subscribe<ProgressiveTaskStartNotification>(StartNotificationHandler);
                _progressiveTaskPerformStepSubscription = _publisherSubscriber.Subscribe<ProgressiveTaskPerformStepNotification>(PerformStepNotificationHandler);
                _progressiveTaskCompleteSubscription = _publisherSubscriber.Subscribe<ProgressiveTaskCompleteNotification>(CompleteNotificationHandler);
                _progressiveTaskCancelSubscription = _publisherSubscriber.Subscribe<ProgressiveTaskCancelNotification>(CancelNotificationHandler);
                _progressiveTaskErrorSubscription = _publisherSubscriber.Subscribe<ProgressiveTaskErrorNotification>(ErrorNotificationHandler);
            }

            private void UnsubscriberFromNotifications() {
                _publisherSubscriber.Unsubscribe(_progressiveTaskStartSubscription);
                _publisherSubscriber.Unsubscribe(_progressiveTaskPerformStepSubscription);
                _publisherSubscriber.Unsubscribe(_progressiveTaskCompleteSubscription);
                _publisherSubscriber.Unsubscribe(_progressiveTaskCancelSubscription);
                _publisherSubscriber.Unsubscribe(_progressiveTaskErrorSubscription);
            }

            private void StartNotificationHandler(ProgressiveTaskStartNotification message) {
                progressTitleLabel.SafeInvoke(_ => _.Text = message.Arguments.Title);
                progressMessageLabel.SafeInvoke(_ => _.Text = message.Arguments.Message);
                mainProgressBar.SafeInvoke(_ => {
                    _.Value = 0;
                    _.Minimum = message.Arguments.ActualStep;
                    _.Maximum = message.Arguments.TotalSteps;
                });
                closeButton.SafeInvoke(_ => _.Text = Resource.ProgressiveTaskForm_ActionButton_Cancel);

                _running = true;
            }

            private void PerformStepNotificationHandler(ProgressiveTaskPerformStepNotification message) {
                progressMessageLabel.SafeInvoke(_ => _.Text = message.Arguments.Message);
                mainProgressBar.SafeInvoke(_ => _.PerformStep());
            }

            private void CompleteNotificationHandler(ProgressiveTaskCompleteNotification message) {
                progressMessageLabel.SafeInvoke(_ => _.Text = message.Arguments.HasError ? message.Arguments.Error : string.Empty);
                closeButton.SafeInvoke(_ => _.Text = Resource.ProgressiveTaskForm_ActionButton_Close);

                _running = false;
            }

            private void CancelNotificationHandler(ProgressiveTaskCancelNotification message) {
                if (_error) { return; }

                progressMessageLabel.SafeInvoke(_ => _.Text = message.Arguments.Message);
                closeButton.SafeInvoke(_ => _.Text = Resource.ProgressiveTaskForm_ActionButton_Close);

                _running = false;
            }

            private void ErrorNotificationHandler(ProgressiveTaskErrorNotification message) {
                progressMessageLabel.SafeInvoke(_ => _.Text = message.Arguments.Error);
                closeButton.SafeInvoke(_ => _.Text = Resource.ProgressiveTaskForm_ActionButton_Close);

                _running = false;
                _error = true;

                _cancellationTokenIssuer.Cancel(PROGRESSIVE_TASK_CANCELLATION_KEY);
            }

            #endregion Private Methods

            #region Event Handlers

            private void ProgressiveTaskForm_Load(object sender, EventArgs e) {
                Initialize();
                SubscriberForNotifications();
            }

            private void ProgressiveTaskForm_FormClosing(object sender, FormClosingEventArgs e) {
                if (DialogResult == DialogResult.OK) { return; }

                var response = MessageBox.Show(Resource.ProgressiveTaskForm_Cancellation_Message,
                    Resource.ProgressiveTaskForm_Cancellation_Title,
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (response == DialogResult.No) { e.Cancel = true; return; }

                _cancellationTokenIssuer.Cancel(PROGRESSIVE_TASK_CANCELLATION_KEY);

                UnsubscriberFromNotifications();
            }

            private void closeButton_Click(object sender, EventArgs e) {
                DialogResult = _running ? DialogResult.Cancel : DialogResult.OK;
            }

            #endregion Event Handlers

            #region Windows Form Designer generated code

            /// <summary>
            /// Required method for Designer support - do not modify
            /// the contents of this method with the code editor.
            /// </summary>
            private void InitializeComponent() {
                progressTitleLabel = new Label();
                progressMessageLabel = new Label();
                mainProgressBar = new ProgressBar();
                titlePanel.SuspendLayout();
                ((ISupportInitialize)(iconPictureBox)).BeginInit();
                contentPanel.SuspendLayout();
                actionPanel.SuspendLayout();
                SuspendLayout();
                //
                // contentPanel
                //
                contentPanel.Controls.Add(mainProgressBar);
                contentPanel.Controls.Add(progressMessageLabel);
                contentPanel.Controls.Add(progressTitleLabel);
                contentPanel.Location = new Point(0, 0);
                contentPanel.Padding = new Padding(15);
                contentPanel.Size = new Size(434, 139);
                //
                // bottomSeparator
                //
                bottomSeparator.Location = new Point(0, 139);
                bottomSeparator.Size = new Size(434, 2);
                //
                // actionPanel
                //
                actionPanel.Location = new Point(0, 141);
                actionPanel.Size = new Size(434, 80);
                //
                // closeButton
                //
                closeButton.Location = new Point(294, 20);
                closeButton.Click += new EventHandler(closeButton_Click);
                //
                // progressTitleLabel
                //
                progressTitleLabel.AutoEllipsis = true;
                progressTitleLabel.Dock = DockStyle.Top;
                progressTitleLabel.Font = new Font("Tahoma", 15.75F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
                progressTitleLabel.Location = new Point(15, 15);
                progressTitleLabel.Name = "progressTitleLabel";
                progressTitleLabel.Size = new Size(404, 32);
                progressTitleLabel.TabIndex = 0;
                progressTitleLabel.Text = "###";
                progressTitleLabel.TextAlign = ContentAlignment.MiddleLeft;
                //
                // progressMessageLabel
                //
                progressMessageLabel.AutoEllipsis = true;
                progressMessageLabel.Dock = DockStyle.Top;
                progressMessageLabel.Font = new Font("Tahoma", 12F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
                progressMessageLabel.Location = new Point(15, 47);
                progressMessageLabel.Name = "progressMessageLabel";
                progressMessageLabel.Padding = new Padding(0, 10, 0, 10);
                progressMessageLabel.Size = new Size(404, 60);
                progressMessageLabel.TabIndex = 1;
                progressMessageLabel.Text = "###";
                //
                // mainProgressBar
                //
                mainProgressBar.Dock = DockStyle.Fill;
                mainProgressBar.Location = new Point(15, 107);
                mainProgressBar.Name = "mainProgressBar";
                mainProgressBar.Size = new Size(404, 17);
                mainProgressBar.TabIndex = 2;
                //
                // ProgressiveTaskForm
                //
                AutoScaleDimensions = new SizeF(10F, 18F);
                AutoScaleMode = AutoScaleMode.Font;
                ClientSize = new Size(434, 221);
                ControlBox = false;
                FormBorderStyle = FormBorderStyle.FixedSingle;
                Name = "ProgressiveTaskForm";
                ShowHeader = false;
                Text = "Executando tarefa...";
                FormClosing += new FormClosingEventHandler(ProgressiveTaskForm_FormClosing);
                Load += new EventHandler(ProgressiveTaskForm_Load);
                titlePanel.ResumeLayout(false);
                ((ISupportInitialize)(iconPictureBox)).EndInit();
                contentPanel.ResumeLayout(false);
                actionPanel.ResumeLayout(false);
                ResumeLayout(false);
            }

            #endregion Windows Form Designer generated code
        }

        #endregion Private Inner Classes
    }
}