using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Autofac;
using InfoFenix.Core.Logging;
using Resource = InfoFenix.Client.Properties.Resources;

namespace InfoFenix.Client.Code {

    public sealed class WaitFor {

        #region Public Static Read-Only Fields

        public static readonly WaitFor Instance = new WaitFor();

        #endregion Public Static Read-Only Fields

        #region Private Constructors

        private WaitFor() {
        }

        #endregion Private Constructors

        #region Public Methods

        public TResult Execution<TResult>(Func<TResult> action, ILogger log = null, bool silent = false) {
            var task = new Task<TResult>(action);
            using (var form = new WaitForm(log, silent)) {
                task
                    .ContinueWith(Continuation, form)
                    .Start();

                form.ShowDialog();
            }
            return task.Result;
        }

        public void Execution(Action action, ILogger log = null, bool silent = false) {
            using (var form = new WaitForm(log, silent)) {
                Task
                    .Run(action)
                    .ContinueWith(Continuation, form);

                form.ShowDialog();
            }
        }

        #endregion Public Methods

        #region Private Static Methods

        private static void Continuation(Task continuation, object state) {
            var form = ((WaitForm)state);
            var fault = continuation.Exception != null;

            if (fault) { form.Log.Error(continuation.Exception.InnerException, continuation.Exception.InnerException.Message); }

            if (!form.Silent) {
                if (fault) {
                    MessageBox.Show(string.Format(Resource.WaitForForm_Error_Message, continuation.Exception.InnerException.Message),
                        Resource.Error,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

                } else {
                    MessageBox.Show(Resource.WaitForForm_Success_Message,
                        Resource.Success,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            ((WaitForm)state).DialogResult = DialogResult.OK;
        }

        #endregion Private Static Methods

        #region Private Inner Classes

        private class WaitForm : Form {

            #region Private Fields

            private Panel waitPanel;
            private PictureBox waitPictureBox;
            private Label waitLabel;

            /// <summary>
            /// Required designer variable.
            /// </summary>
            private IContainer components = null;

            #endregion Private Fields

            #region Public Properties

            public ILogger Log { get; }
            public bool Silent { get; }

            #endregion Public Properties

            #region Public Constructors

            public WaitForm(ILogger log = null, bool silent = false) {
                Log = log ?? NullLogger.Instance;
                Silent = silent;

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

            /// <summary>
            /// Required method for Designer support - do not modify
            /// the contents of this method with the code editor.
            /// </summary>
            private void InitializeComponent() {
                waitPanel = new Panel();
                waitLabel = new Label();
                waitPictureBox = new PictureBox();
                waitPanel.SuspendLayout();
                ((System.ComponentModel.ISupportInitialize)(waitPictureBox)).BeginInit();
                SuspendLayout();
                //
                // waitPanel
                //
                waitPanel.BorderStyle = BorderStyle.FixedSingle;
                waitPanel.Controls.Add(waitLabel);
                waitPanel.Controls.Add(waitPictureBox);
                waitPanel.Dock = DockStyle.Fill;
                waitPanel.ForeColor = System.Drawing.SystemColors.ControlText;
                waitPanel.Location = new System.Drawing.Point(7, 7);
                waitPanel.Name = "waitPanel";
                waitPanel.Padding = new Padding(7);
                waitPanel.Size = new System.Drawing.Size(156, 48);
                waitPanel.TabIndex = 0;
                //
                // waitLabel
                //
                waitLabel.Dock = DockStyle.Fill;
                waitLabel.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                waitLabel.Location = new System.Drawing.Point(39, 7);
                waitLabel.Margin = new Padding(0);
                waitLabel.Name = "waitLabel";
                waitLabel.Size = new System.Drawing.Size(108, 32);
                waitLabel.TabIndex = 1;
                waitLabel.Text = "Aguarde...";
                waitLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
                //
                // waitPictureBox
                //
                waitPictureBox.Dock = DockStyle.Left;
                waitPictureBox.Image = Resource.hourglass_32x32;
                waitPictureBox.Location = new System.Drawing.Point(7, 7);
                waitPictureBox.Margin = new Padding(0);
                waitPictureBox.Name = "waitPictureBox";
                waitPictureBox.Size = new System.Drawing.Size(32, 32);
                waitPictureBox.TabIndex = 0;
                waitPictureBox.TabStop = false;
                //
                // WaitForm
                //
                AutoScaleMode = AutoScaleMode.None;
                ClientSize = new System.Drawing.Size(170, 62);
                Controls.Add(waitPanel);
                Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                FormBorderStyle = FormBorderStyle.None;
                Name = "WaitForm";
                Padding = new Padding(7);
                StartPosition = FormStartPosition.CenterScreen;
                waitPanel.ResumeLayout(false);
                ((System.ComponentModel.ISupportInitialize)(waitPictureBox)).EndInit();
                ResumeLayout(false);
            }

            #endregion Private Methods
        }

        #endregion Private Inner Classes
    }
}