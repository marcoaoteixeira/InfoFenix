using InfoFenix.Core;
using System.Collections.Generic;
using System.Linq;
using InfoFenix.Core.PubSub;

namespace InfoFenix.Core.Bootstrap {
    public sealed class Bootstrapper : IBootstrapper {

        #region Private Read-Only Fields

        private readonly IEnumerable<IAction> _actions;
        private readonly IPublisherSubscriber _publisherSubscriber;

        #endregion Private Read-Only Fields

        #region Public Constructors

        public Bootstrapper(IEnumerable<IAction> actions, IPublisherSubscriber publisherSubscriber) {
            Prevent.ParameterNull(actions, nameof(actions));

            _actions = actions;
            _publisherSubscriber = publisherSubscriber;
        }

        #endregion Public Constructors

        #region IBootstrapper Members

        public void Run() {
            var lastOrder = new OrderAttribute(int.MaxValue);
            var actions = _actions.OrderBy(_ => _.GetType().GetCustomAttribute(fallback: lastOrder).Order).ToArray();

            _publisherSubscriber.Publish(new ProgressiveTaskStartNotification {
                Message = "Iniciando aplicativo...",
                TotalSteps = actions.Length
            });
            actions.Each((_, idx) => {
                _publisherSubscriber.Publish(new ProgressiveTaskPerformStepNotification {
                    Message = $"Tarefa: {_.Name}",
                    ActualStep = (idx + 1),
                    TotalSteps = actions.Length
                });
                _.Execute();
            });
            _publisherSubscriber.Publish(new ProgressiveTaskCompleteNotification {
                Message = "Inicialização concluída! Por favor, aguarde...",
                TotalSteps = actions.Length
            });
        }

        #endregion IBootstrapper Members
    }
}