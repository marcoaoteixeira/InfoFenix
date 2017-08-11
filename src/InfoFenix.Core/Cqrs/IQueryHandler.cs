using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfoFenix.Core.CQRS {

    public interface IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult> {

        #region Methods

        Task<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken = default(CancellationToken));

        #endregion Methods
    }
}