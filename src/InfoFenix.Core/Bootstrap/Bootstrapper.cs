using InfoFenix.Core;
using System.Collections.Generic;
using System.Linq;

namespace InfoFenix.Core.Bootstrap {
    public sealed class Bootstrapper : IBootstrapper {

        #region Private Read-Only Fields

        private readonly IEnumerable<IAction> _actions;

        #endregion Private Read-Only Fields

        #region Public Constructors

        public Bootstrapper(IEnumerable<IAction> actions) {
            Prevent.ParameterNull(actions, nameof(actions));

            _actions = actions;
        }

        #endregion Public Constructors

        #region IBootstrapper Members

        public void Run() {
            var lastOrder = new OrderAttribute(int.MaxValue);
            var actions = _actions.OrderBy(_ => _.GetType().GetCustomAttribute(fallback: lastOrder).Order).ToArray();

            actions.Each(_ => _.Execute());
        }

        #endregion IBootstrapper Members
    }
}