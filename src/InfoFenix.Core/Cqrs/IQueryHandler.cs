namespace InfoFenix.Core.Cqrs {
    public interface IQueryHandler<in TQuery, out TResult> where TQuery : IQuery<TResult> {

        #region Methods

        TResult Handle(TQuery query);

        #endregion Methods
    }
}