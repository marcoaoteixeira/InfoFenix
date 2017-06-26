﻿using System;
using System.Threading;
using System.Threading.Tasks;
using InfoFenix.Core.IoC;

namespace InfoFenix.Core.Cqrs {

    public sealed class Mediator : IMediator {

        #region Private Read-Only Fields

        private readonly IResolver _resolver;

        #endregion Private Read-Only Fields

        #region Public Constructors

        public Mediator(IResolver resolver) {
            Prevent.ParameterNull(resolver, nameof(resolver));

            _resolver = resolver;
        }

        #endregion Public Constructors

        #region ICommandQueryDispatcher Members

        public Task CommandAsync(ICommand command, IProgress<ProgressArguments> progress = null, CancellationToken cancellationToken = default(CancellationToken)) {
            Prevent.ParameterNull(command, nameof(command));

            var handlerType = typeof(ICommandHandler<>).MakeGenericType(command.GetType());
            dynamic handler = _resolver.Resolve(handlerType);

            return handler.HandleAsync((dynamic)command, progress ?? NullProgress<ProgressArguments>.Instance, cancellationToken);
        }

        public Task<TResult> QueryAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default(CancellationToken)) {
            Prevent.ParameterNull(query, nameof(query));

            var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));
            dynamic handler = _resolver.Resolve(handlerType);

            return handler.HandleAsync((dynamic)query, cancellationToken);
        }

        #endregion ICommandQueryDispatcher Members
    }
}