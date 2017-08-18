using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfoFenix.CQRS {
    public interface ICommandHandler<in TCommand> where TCommand : ICommand {

        #region Methods

        Task HandleAsync(TCommand command, CancellationToken cancellationToken = default(CancellationToken), IProgress<ProgressInfo> progress = null);

        #endregion Methods
    }
}