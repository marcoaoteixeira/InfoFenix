using System;
using System.Threading;
using System.Threading.Tasks;

namespace InfoFenix.Core.Cqrs {
    public interface ICommandHandler<in TCommand> where TCommand : ICommand {

        #region Methods

        Task HandleAsync(TCommand command, IProgress<ProgressArguments> progress = null, CancellationToken cancellationToken = default(CancellationToken));

        #endregion Methods
    }
}