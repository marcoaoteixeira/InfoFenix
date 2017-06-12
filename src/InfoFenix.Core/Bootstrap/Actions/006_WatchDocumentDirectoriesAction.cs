using InfoFenix.Core.Logging;
using InfoFenix.Core.Services;

namespace InfoFenix.Core.Bootstrap.Actions {

    [Order(6)]
    public class WatchDocumentDirectoriesAction : ActionBase {

        #region Private Read-Only Fields

        private readonly IDocumentDirectoryService _documentDirectoryService;
        private readonly ILogger _log;

        #endregion Private Read-Only Fields

        #region Public Constructors

        public WatchDocumentDirectoriesAction(IDocumentDirectoryService documentDirectoryService, ILogger log) {
            Prevent.ParameterNull(documentDirectoryService, nameof(documentDirectoryService));

            _documentDirectoryService = documentDirectoryService;
            _log = log ?? NullLogger.Instance;
        }

        #endregion Public Constructors

        #region IAction Members

        public override string Name => "Observar Diretórios de Documentos";

        public override void Execute() {
            var documentDirectories = _documentDirectoryService.List();
            foreach (var documentDirectory in documentDirectories) {
                _documentDirectoryService.StartWatchForModification(documentDirectory.ID);
            }
        }

        #endregion IAction Members
    }
}