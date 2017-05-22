namespace InfoFenix.Core.Bootstrap {
    public abstract class ActionBase : IAction {

        #region IAction Members

        public abstract void Execute();

        #endregion IAction Members

        #region Public Override Methods

        public override string ToString() {
            return $"Order: {GetType().GetCustomAttribute(fallback: new OrderAttribute(int.MaxValue)).Order} | Action: {GetType().Name}";
        }

        #endregion Public Override Methods
    }
}