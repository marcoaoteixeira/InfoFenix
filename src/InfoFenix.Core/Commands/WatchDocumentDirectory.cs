using InfoFenix.Core.Cqrs;
using InfoFenix.Core.IO;

namespace InfoFenix.Core.Commands {

    public class WatchDocumentDirectoryCommand : ICommand {

        #region Public Properties

        public string DirectoryPath { get; set; }

        #endregion Public Properties
    }

    public class WatchDocumentDirectoryCommandHandler : ICommandHandler<WatchDocumentDirectoryCommand> {

        #region Private Read-Only Fields

        private readonly IDirectoryWatcherManager _manager;

        #endregion Private Read-Only Fields

        #region Public Constructors

        public WatchDocumentDirectoryCommandHandler(IDirectoryWatcherManager manager) {
            Prevent.ParameterNull(manager, nameof(manager));

            _manager = manager;
        }

        #endregion Public Constructors

        #region ICommandHandler<WatchDocumentDirectoryCommand> Members

        public void Handle(WatchDocumentDirectoryCommand command) {
            _manager.StartWatch(command.DirectoryPath);
        }

        #endregion ICommandHandler<WatchDocumentDirectoryCommand> Members
    }
}