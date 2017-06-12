using System;
using System.Threading;
using System.Threading.Tasks;
using InfoFenix.Core;

namespace InfoFenix.Client.Code.Jobs {

    public interface IJob {

        #region Properties

        string Description { get; set; }

        #endregion Properties

        #region Methods

        void ContinueWith(IJob job);

        void Run(CancellationToken cancellationToken);

        #endregion Methods
    }

    public static class JobExtension {

        #region Public Static Methods

        public static void Run(this IJob source) {
            if (source == null) { return; }

            source.Run(CancellationToken.None);
        }

        #endregion Public Static Methods
    }

    public class Job : IJob {

        #region Private Fields

        private IJob _next;

        private Action<CancellationToken> _action;

        #endregion Private Fields

        #region Public Constructors

        public Job(Action action) {
            Prevent.ParameterNull(action, nameof(action));

            _action = (_) => action();
        }

        public Job(Action<CancellationToken> action) {
            Prevent.ParameterNull(action, nameof(action));

            _action = action;
        }

        #endregion Public Constructors

        #region Private Methods

        private void Continuation(Task task, object state) {
            if (task.Exception != null) {
                // Log error?
            }

            if (_next != null && !task.IsCanceled) {
                _next.Run((CancellationToken)state);
            }

            var taskA = new Task(() => Console.Write(""));

            
        }

        #endregion Private Methods

        #region IJob Members

        public string Description { get; set; }

        public void ContinueWith(IJob job) {
            _next = job;
        }

        public void Run(CancellationToken cancellationToken) {
            Task
                .Run(() => _action(cancellationToken))
                .ContinueWith(Continuation, cancellationToken);
        }

        #endregion IJob Members
    }
}