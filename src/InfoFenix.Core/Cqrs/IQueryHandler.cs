using System.Threading;
using System.Threading.Tasks;

namespace InfoFenix.Core.Cqrs {

    public interface IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult> {

        #region Methods

        Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken = default(CancellationToken));

        #endregion Methods
    }
}