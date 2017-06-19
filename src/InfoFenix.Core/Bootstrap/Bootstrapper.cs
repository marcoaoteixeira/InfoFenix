using System;
using System.Collections.Generic;
using System.Linq;
using InfoFenix.Core;
using InfoFenix.Core.Logging;
using InfoFenix.Core.PubSub;

namespace InfoFenix.Core.Bootstrap {

    public sealed class Bootstrapper : IBootstrapper {

        #region Private Read-Only Fields

        private readonly IEnumerable<IAction> _actions;
        private readonly IPublisherSubscriber _publisherSubscriber;

        #endregion Private Read-Only Fields

        #region Public Properties

        private ILogger _log;

        public ILogger Log {
            get { return _log ?? NullLogger.Instance; }
            set { _log = value ?? NullLogger.Instance; }
        }

        #endregion Public Properties

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
            var actualStep = 0;

            _publisherSubscriber.ProgressiveTaskStartAsync(
                title: Resources.Resources.Bootstrapper_ProgressiveTaskStart_Title,
                totalSteps: actions.Length
            );

            actions.Each((_, idx) => {
                _publisherSubscriber.ProgressiveTaskPerformStepAsync(
                    message: string.Format(Resources.Resources.Bootstrapper_ProgressiveTaskPerformStep_Message, _.Name),
                    actualStep: ++actualStep,
                    totalSteps: actions.Length
                );

                try { _.Execute(); } catch (Exception ex) {
                    _publisherSubscriber.ProgressiveTaskErrorAsync(
                        actualStep: actualStep,
                        error: ex.Message,
                        totalSteps: actions.Length
                    );

                    Log.Error(ex, ex.Message);

                    throw;
                }
            });
            _publisherSubscriber.ProgressiveTaskCompleteAsync(
                message: Resources.Resources.Bootstrapper_ProgressiveTaskComplete_Message,
                actualStep: actualStep,
                totalSteps: actions.Length
            );
        }

        #endregion IBootstrapper Members
    }
}