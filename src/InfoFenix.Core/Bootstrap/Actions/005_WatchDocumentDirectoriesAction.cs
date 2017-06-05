using System.Linq;
using InfoFenix.Core.Commands;
using InfoFenix.Core.Cqrs;
using InfoFenix.Core.Logging;
using InfoFenix.Core.Queries;

namespace InfoFenix.Core.Bootstrap.Actions {

    [Order(5)]
    public class WatchDocumentDirectoriesAction : ActionBase {

        #region Private Read-Only Fields

        private readonly ICommandQueryDispatcher _commandQueryDispatcher;
        private readonly ILogger _log;

        #endregion Private Read-Only Fields

        #region Public Constructors

        public WatchDocumentDirectoriesAction(ICommandQueryDispatcher commandQueryDispatcher, ILogger log) {
            Prevent.ParameterNull(commandQueryDispatcher, nameof(commandQueryDispatcher));

            _commandQueryDispatcher = commandQueryDispatcher;
            _log = log ?? NullLogger.Instance;
        }

        #endregion Public Constructors

        #region IAction Members

        public override string Description => "Observação de Diretório de Índices";

        public override void Execute() {
            var documentDirectories = _commandQueryDispatcher.Query(new ListDocumentDirectoriesQuery()).ToArray();
            foreach (var documentDirectory in documentDirectories) {
                _commandQueryDispatcher.Command(new StartWatchDocumentDirectoryCommand {
                    DirectoryPath = documentDirectory.Path
                });
            }
        }

        #endregion IAction Members
    }
}