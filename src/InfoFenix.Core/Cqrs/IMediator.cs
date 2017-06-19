﻿using System.Threading;
using System.Threading.Tasks;

namespace InfoFenix.Core.Cqrs {

    public interface IMediator {

        #region Methods

        Task CommandAsync(ICommand command, CancellationToken cancellationToken = default(CancellationToken));

        Task<TResult> QueryAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default(CancellationToken));

        #endregion Methods
    }
}