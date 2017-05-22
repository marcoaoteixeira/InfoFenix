namespace InfoFenix.Core.Cqrs {

    public interface ICommandHandler<in TCommand> where TCommand : ICommand {

        #region Methods

        void Handle(TCommand command);

        #endregion Methods
    }
}