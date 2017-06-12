using System;
using System.Threading;
using System.Windows.Forms;

namespace InfoFenix.Client.Views {

    public partial class TaskForm : Form {

        #region Public Properties

        public Action<CancellationToken>[] Actions { get; set; }

        #endregion

        #region Public Constructors

        public TaskForm() {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Public Static Methods

        public static void Prepare(Action<CancellationToken>[] actions) {

        }

        #endregion

        #region Private Methods

        private void Initialize() {
        }

        #endregion

        #region Event Handlers

        private void TaskForm_Load(object sender, EventArgs e) {
            Initialize();
        }

        #endregion
    }
}