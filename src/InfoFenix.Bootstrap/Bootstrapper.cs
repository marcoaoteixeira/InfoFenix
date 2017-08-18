using System;
using System.Collections.Generic;
using System.Linq;
using InfoFenix.Logging;
using InfoFenix.PubSub;
using InfoFenix.Resources;

namespace InfoFenix.Bootstrap {

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
            var actions = _actions
                .Where(_ => !_.GetType().GetCustomAttribute(fallback: lastOrder).Ignore)
                .OrderBy(_ => _.GetType().GetCustomAttribute(fallback: lastOrder).Order)
                .ToArray();

            var arguments = new ProgressiveTaskArguments {
                ActualStep = 0,
                TotalSteps = actions.Length
            };

            _publisherSubscriber.ProgressiveTaskStart(
                title: Strings.Bootstrapper_ProgressiveTaskStart_Title,
                actualStep: arguments.ActualStep,
                totalSteps: arguments.TotalSteps
            );

            actions.Each((_, idx) => {
                try {
                    _publisherSubscriber.ProgressiveTaskPerformStep(
                        message: string.Format(Strings.Bootstrapper_ProgressiveTaskPerformStep_Message, _.Name),
                        actualStep: ++arguments.ActualStep,
                        totalSteps: arguments.TotalSteps
                    );

                    _.Execute();
                } catch (Exception ex) {
                    _publisherSubscriber.ProgressiveTaskError(
                        actualStep: arguments.ActualStep,
                        error: ex.Message,
                        totalSteps: arguments.TotalSteps
                    );

                    Log.Error(ex.Message);

                    throw new FatalException(Strings.FatalException_Message, ex);
                }
            });

            _publisherSubscriber.ProgressiveTaskComplete(
                message: Strings.Bootstrapper_ProgressiveTaskComplete_Message,
                actualStep: arguments.ActualStep,
                totalSteps: arguments.TotalSteps
            );
        }

        #endregion IBootstrapper Members
    }
}