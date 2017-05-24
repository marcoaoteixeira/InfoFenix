using InfoFenix.Core.Cqrs;
using InfoFenix.Core.IO;

namespace InfoFenix.Core.Commands {

    public class StartWatchDocumentDirectoryCommand : ICommand {

        #region Public Properties

        public string DirectoryPath { get; set; }

        #endregion Public Properties
    }

    public class StartWatchDocumentDirectoryCommandHandler : ICommandHandler<StartWatchDocumentDirectoryCommand> {

        #region Private Read-Only Fields

        private readonly IDirectoryWatcherManager _manager;

        #endregion Private Read-Only Fields

        #region Public Constructors

        public StartWatchDocumentDirectoryCommandHandler(IDirectoryWatcherManager manager) {
            Prevent.ParameterNull(manager, nameof(manager));

            _manager = manager;
        }

        #endregion Public Constructors

        #region ICommandHandler<StartWatchDocumentDirectoryCommand> Members

        public void Handle(StartWatchDocumentDirectoryCommand command) {
            _manager.StartWatch(command.DirectoryPath);
        }

        #endregion ICommandHandler<StartWatchDocumentDirectoryCommand> Members
    }
}