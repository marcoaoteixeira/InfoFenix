using InfoFenix.Core.Logging;

namespace InfoFenix.Core.Bootstrap {

    public abstract class ActionBase : IAction {

        #region Public Properties

        private ILogger _log;

        public ILogger Log {
            get { return _log ?? NullLogger.Instance; }
            set { _log = value ?? NullLogger.Instance; }
        }

        #endregion Public Properties

        #region IAction Members

        public abstract string Name { get; }

        public abstract void Execute();

        #endregion IAction Members

        #region Public Override Methods

        public override string ToString() {
            return $"Order: {GetType().GetCustomAttribute(fallback: new OrderAttribute(int.MaxValue)).Order} | Action: {GetType().Name}";
        }

        #endregion Public Override Methods
    }
}