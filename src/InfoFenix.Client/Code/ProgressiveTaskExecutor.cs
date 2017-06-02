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

        #region Private Fields

        private CancellationToken _currentCancellationToken;

        #endregion Private Fields

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

        public void Execute(Action<CancellationToken> action) {
            using (var form = _formManager.Get<ProgressForm>()) {
                form.Initialize();

                form.CancelProgressiveTask += Form_CancelProgressiveTask;
                form.CompleteProgressiveTask += Form_CompleteProgressiveTask;

                _currentCancellationToken = _cancellationTokenIssuer.Get(DefaultCancellationTokenKey);
                _currentCancellationToken.Register(CancelCallback);

                Task
                    .Run(() => action(_currentCancellationToken))
                    .ContinueWith(Continuation, form);

                form.ShowDialog();
            }
        }

        private void Form_CancelProgressiveTask(object sender, EventArgs e) {
            var response = MessageBox.Show("Deseja cancelar a tarefa?", "Cancelar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (response == DialogResult.No) { return; }

            _cancellationTokenIssuer.Cancel(DefaultCancellationTokenKey);

            var form = sender as Form;
            if (form == null) { return; }

            form.DialogResult = DialogResult.OK;
        }

        private void Form_CompleteProgressiveTask(object sender, EventArgs e) {
            var form = sender as Form;

            if (_currentCancellationToken != null && !_currentCancellationToken.IsCancellationRequested) {
                MessageBox.Show("A tarefa foi concluída com sucesso.", "Concluído", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            _cancellationTokenIssuer.MarkAsComplete(DefaultCancellationTokenKey);

            form.DialogResult = DialogResult.OK;
        }

        private void CancelCallback() {
            MessageBox.Show("A tarefa foi cancelada.", "Cancelado", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

        private void Continuation(Task continuationTask, object state) {
            var exception = continuationTask.Exception;
            if (exception != null) {
                Log.Error(exception, exception.Message);

                MessageBox.Show($"Ocorreram erros durante a execução da tarefa.{Environment.NewLine}Veja o log de aplicativo para mais informações.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        #endregion Public Methods
    }
}