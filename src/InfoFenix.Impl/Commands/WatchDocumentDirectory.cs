using InfoFenix.Core;
using InfoFenix.Core.Cqrs;
using InfoFenix.Core.IO;

namespace InfoFenix.Impl.Commands {
    public class WatchDocumentDirectoryCommand : ICommand {
        #region Public Properties

        public int DocumentDirectoryID { get; set; }

        public string DirectoryPath { get; set; }

        #endregion
    }

    public class WatchDocumentDirectoryCommandHandler : ICommandHandler<WatchDocumentDirectoryCommand> {
        #region Private Read-Only Fields

        private readonly IDirectoryWatcherManager _manager;

        #endregion

        #region Public Constructors

        public WatchDocumentDirectoryCommandHandler(IDirectoryWatcherManager manager) {
            Prevent.ParameterNull(manager, nameof(manager));

            _manager = manager;
        }

        #endregion

        #region ICommandHandler<WatchDocumentDirectoryCommand> Members
        public void Handle(WatchDocumentDirectoryCommand command) {
            _manager.StartWatch(command.DirectoryPath);
        }
        #endregion
    }
}
