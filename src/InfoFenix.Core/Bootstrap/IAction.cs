namespace InfoFenix.Core.Bootstrap {
    public interface IAction {

        #region Properties

        string Name { get; }

        #endregion

        #region Methods

        void Execute();

        #endregion Methods
    }
}