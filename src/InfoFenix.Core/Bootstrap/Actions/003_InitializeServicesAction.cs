using InfoFenix.Core.Logging;

namespace InfoFenix.Core.Bootstrap.Actions {

    [Order(3)]
    public class InitializeServicesAction : ActionBase {

        #region Private Read-Only Fields

        private readonly ILogger _log;

        #endregion

        #region Public Constructors

        public InitializeServicesAction(ILogger log) {
            _log = log ?? NullLogger.Instance;
        }

        #endregion Public Constructors

        #region IAction Members

        public override void Execute() {
            _log.Information("Initializing services...");

            _log.Information("Services initialized!");
        }

        #endregion IAction Members
    }
}