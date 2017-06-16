using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace InfoFenix.Client.Views.Shared {

    public partial class TaskItemUserControl : UserControl {

        #region Public Enum

        public enum TaskIcon {
            None = 0,
            InProgress = 1,
            Complete = 2,
            Error = 3
        }

        #endregion Public Enum

        #region Public Properties

        private TaskIcon _icon;

        [Browsable(true)]
        public TaskIcon Icon {
            get { return _icon; }
            set {
                _icon = value;
                SetPictureBoxImage(_icon);
            }
        }

        [Browsable(true)]
        public string Title {
            get { return titleLabel.Text; }
            set { titleLabel.Text = value; }
        }

        [Browsable(true)]
        public string Message {
            get { return messageLabel.Text; }
            set { messageLabel.Text = value; }
        }

        #endregion Public Properties

        #region Public Constructors

        public TaskItemUserControl() {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Public Methods

        public void SetComplete() {
            Icon = TaskIcon.Complete;
            titleLabel.Font = new Font(titleLabel.Font, FontStyle.Bold);
        }

        #endregion Public Methods

        #region Private Methods

        private void Initialize() {
            SetPictureBoxImage(_icon);
        }

        private void SetPictureBoxImage(TaskIcon icon) {
            mainPictureBox.Image = mainImageList.Images[(int)icon];
        }

        #endregion Private Methods

        #region Event Handlers

        private void TaskItemUserControl_Load(object sender, EventArgs e) {
            Initialize();
        }

        #endregion Event Handlers
    }
}