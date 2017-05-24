using InfoFenix.Core.Cqrs;
using InfoFenix.Core.IO;

namespace InfoFenix.Core.Commands {

    public class StopWatchDocumentDirectoryCommand : ICommand {

        #region Public Properties

        public string DirectoryPath { get; set; }

        #endregion Public Properties
    }

    public class StopWatchDocumentDirectoryCommandHandler : ICommandHandler<StopWatchDocumentDirectoryCommand> {

        #region Private Read-Only Fields

        private readonly IDirectoryWatcherManager _manager;

        #endregion Private Read-Only Fields

        #region Public Constructors

        public StopWatchDocumentDirectoryCommandHandler(IDirectoryWatcherManager manager) {
            Prevent.ParameterNull(manager, nameof(manager));

            _manager = manager;
        }

        #endregion Public Constructors

        #region ICommandHandler<StopWatchDocumentDirectoryCommand> Members

        public void Handle(StopWatchDocumentDirectoryCommand command) {
            _manager.StopWatch(command.DirectoryPath);
        }

        #endregion ICommandHandler<StopWatchDocumentDirectoryCommand> Members
    }
}