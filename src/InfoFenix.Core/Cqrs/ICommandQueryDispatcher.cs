namespace InfoFenix.Core.Cqrs {
    public interface ICommandQueryDispatcher {

        #region Methods

        void Command(ICommand command);

        TResult Query<TResult>(IQuery<TResult> query);

        #endregion Methods
    }
}