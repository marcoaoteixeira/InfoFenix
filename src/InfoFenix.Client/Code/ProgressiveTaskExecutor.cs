using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using InfoFenix.Client.Views;
using InfoFenix.Core;
using InfoFenix.Core.Logging;

namespace InfoFenix.Client.Code {

    public sealed class ProgressiveTaskExecutor {

        #region Private Static Read-Only Fields

        private static readonly string DefaultCancellationTokenKey = $"{nameof(ProgressiveTaskExecutor)}::EXECUTING_TASK";

        #endregion Private Static Read-Only Fields

        #region Private Read-Only Fields

        private readonly CancellationTokenIssuer _cancellationTokenIssuer;
        private readonly IFormManager _formManager;

        #endregion Private Read-Only Fields

        #region Public Properties

        private ILogger _log;

        public ILogger Log {
            get { return _log ?? NullLogger.Instance; }
            set { _log = value ?? NullLogger.Instance; }
        }

        #endregion Public Properties

        #region Public Constructors

        public ProgressiveTaskExecutor(CancellationTokenIssuer cancellationTokenIssuer, IFormManager formManager) {
            _cancellationTokenIssuer = cancellationTokenIssuer;
            _formManager = formManager;
        }

        #endregion Public Constructors

        #region Public Methods

        public void Execute(Action action) {
            using (var form = _formManager.Get<ProgressiveTaskForm>()) {
                form.Initialize(canCancel: false);

                form.CompleteProgressiveTask += Form_CompleteProgressiveTask;

                Task
                    .Run(action)
                    .ContinueWith(Continuation);

                form.ShowDialog();
            }
        }

        public void Execute(Action<CancellationToken> action) {
            using (var form = _formManager.Get<ProgressiveTaskForm>()) {
                form.Initialize(canCancel: true);

                form.CancelProgressiveTask += Form_CancelProgressiveTask;
                form.CompleteProgressiveTask += Form_CompleteProgressiveTask;

                Task
                    .Run(() => action(_cancellationTokenIssuer.Get(DefaultCancellationTokenKey)))
                    .ContinueWith(Continuation);

                form.ShowDialog();
            }
        }

        private void Form_CancelProgressiveTask(object sender, ProgressiveTaskCancelEventArgs e) {
            if (!e.Force) {
                var response = MessageBox.Show("Deseja cancelar a tarefa?", "Cancelar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (response == DialogResult.No) { return; }
            }

            _cancellationTokenIssuer.Cancel(DefaultCancellationTokenKey);
        }

        private void Form_CompleteProgressiveTask(object sender, ProgressiveTaskCompleteEventArgs e) {
            _cancellationTokenIssuer.MarkAsComplete(DefaultCancellationTokenKey);
        }

        private void Continuation(Task continuationTask) {
            var exception = continuationTask.Exception;
            if (exception != null) {
                Log.Error(exception, exception.Message);

                throw exception;
            }
        }

        #endregion Public Methods
    }
}