﻿using System.Threading;
using System.Threading.Tasks;

namespace InfoFenix.Core.Cqrs {
    public static class CommandQueryDispatcherExtension {

        #region Public Static Methods

        public static Task CommandAsync(this ICommandQueryDispatcher source, ICommand command) {
            return CommandAsync(source, command, CancellationToken.None);
        }

        public static Task CommandAsync(this ICommandQueryDispatcher source, ICommand command, CancellationToken cancellationToken) {
            if (source == null) { return Task.FromResult(0); }

            return Task.Run(() => source.Command(command), cancellationToken);
        }

        public static Task<TResult> QueryAsync<TResult>(this ICommandQueryDispatcher source, IQuery<TResult> query) {
            return QueryAsync(source, query, CancellationToken.None);
        }

        public static Task<TResult> QueryAsync<TResult>(this ICommandQueryDispatcher source, IQuery<TResult> query, CancellationToken cancellationToken) {
            if (source == null) { return Task.FromResult(default(TResult)); }

            return Task.Run(() => source.Query(query), cancellationToken);
        }

        #endregion Public Static Methods
    }
}