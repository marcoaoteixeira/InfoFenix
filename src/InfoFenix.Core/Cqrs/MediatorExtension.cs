using System.Threading;

namespace InfoFenix.Core.Cqrs {

    public static class MediatorExtension {

        #region Public Static Methods

        public static void Command(this IMediator source, ICommand command) {
            if (source == null) { return; }

            source.CommandAsync(command, CancellationToken.None).WaitForResult();
        }

        public static TResult Query<TResult>(this IMediator source, IQuery<TResult> query) {
            if (source == null) { return default(TResult); }

            return source.QueryAsync(query, CancellationToken.None).WaitForResult();
        }

        #endregion Public Static Methods
    }
}