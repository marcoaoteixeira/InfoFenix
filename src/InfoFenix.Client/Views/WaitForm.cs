using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InfoFenix.Client.Views {

    public partial class WaitForm : Form {

        #region Public Constructors

        public WaitForm() {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Public Static Methods

        public static void WaitFor(Action action) {
            using (var form = new WaitForm()) {
                Task
                    .Run(action)
                    .ContinueWith(Continuation, form);

                form.ShowDialog();
            }
        }

        #endregion Public Static Methods

        #region Private Static Methods

        private static void Continuation(Task continuation, object state) {
            var form = state as WaitForm;

            form.DialogResult = DialogResult.OK;
        }

        #endregion Private Static Methods
    }
}