using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using InfoFenix.Client.Views.Shared;
using InfoFenix.Core.Logging;
using Resource = InfoFenix.Client.Properties.Resources;

namespace InfoFenix.Client.Code {

    public static class WaitFor {

        #region Public Methods

        public static TResult Execution<TResult>(Func<TResult> action, ILogger log = null, bool silent = false) {
            var task = new Task<TResult>(action);
            using (var form = new WaitForForm(log, silent)) {
                task.ContinueWith(Continuation, form);
                task.Start();

                form.ShowDialog();
            }
            return task.Result;
        }

        public static void Execution(Action action, ILogger log = null, bool silent = false) {
            using (var form = new WaitForForm(log, silent)) {
                Task
                    .Run(action)
                    .ContinueWith(Continuation, form);

                form.ShowDialog();
            }
        }

        #endregion Public Methods

        #region Private Static Methods

        private static void Continuation(Task continuation, object state) {
            var form = ((WaitForForm)state);
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
            ((WaitForForm)state).DialogResult = DialogResult.OK;
        }

        #endregion Private Static Methods
    }
}