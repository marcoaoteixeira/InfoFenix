using InfoFenix.Core.IoC;

namespace InfoFenix.Core.Cqrs {
    public sealed class CommandQueryDispatcher : ICommandQueryDispatcher {

        #region Private Read-Only Fields

        private readonly IResolver _resolver;

        #endregion Private Read-Only Fields

        #region Public Constructors

        public CommandQueryDispatcher(IResolver resolver) {
            Prevent.ParameterNull(resolver, nameof(resolver));

            _resolver = resolver;
        }

        #endregion Public Constructors

        #region ICommandQueryDispatcher Members

        public void Command(ICommand command) {
            Prevent.ParameterNull(command, nameof(command));

            var handlerType = typeof(ICommandHandler<>).MakeGenericType(command.GetType());
            dynamic handler = _resolver.Resolve(handlerType);

            handler.Handle((dynamic)command);
        }

        public TResult Query<TResult>(IQuery<TResult> query) {
            Prevent.ParameterNull(query, nameof(query));

            var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));
            dynamic handler = _resolver.Resolve(handlerType);

            return handler.Handle((dynamic)query);
        }

        #endregion ICommandQueryDispatcher Members
    }
}