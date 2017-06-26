using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfoFenix.Core.Cqrs {

    public interface IMediator {

        #region Methods

        Task CommandAsync(ICommand command, IProgress<ProgressArguments> progress = null, CancellationToken cancellationToken = default(CancellationToken));

        Task<TResult> QueryAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default(CancellationToken));

        #endregion Methods
    }
}