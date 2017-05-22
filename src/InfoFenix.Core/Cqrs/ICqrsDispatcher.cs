namespace InfoFenix.Core.Cqrs {
    public interface ICqrsDispatcher {

        #region Methods

        void Command(ICommand command);

        TResult Query<TResult>(IQuery<TResult> query);

        #endregion Methods
    }
}