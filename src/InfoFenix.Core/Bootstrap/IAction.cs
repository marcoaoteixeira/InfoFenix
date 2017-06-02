namespace InfoFenix.Core.Bootstrap {
    public interface IAction {

        #region Properties

        string Description { get; }

        #endregion

        #region Methods

        void Execute();

        #endregion Methods
    }
}