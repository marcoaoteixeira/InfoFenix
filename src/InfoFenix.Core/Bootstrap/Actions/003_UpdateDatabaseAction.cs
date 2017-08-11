using System;
using InfoFenix.Core.CQRS;
using InfoFenix.Core.Logging;
using InfoFenix.Core.Migrations;

namespace InfoFenix.Core.Bootstrap.Actions {

    [Order(3)]
    public class UpdateDatabaseAction : ActionBase {

        #region Private Read-Only Fields

        private readonly IMediator _mediator;

        #endregion Private Read-Only Fields

        #region Public Properties

        private ILogger _log;

        /// <summary>
        /// Gets or sets the Logger value.
        /// </summary>
        public ILogger Logger {
            get { return _log ?? (_log = NullLogger.Instance); }
            set { _log = value ?? NullLogger.Instance; }
        }

        #endregion Public Properties

        #region Public Constructors

        public UpdateDatabaseAction(IMediator mediator) {
            Prevent.ParameterNull(mediator, nameof(mediator));

            _mediator = mediator;
        }

        #endregion Public Constructors

        #region IAction Members

        public override string Name => "Atualizando Banco de Dados";

        public override void Execute() {
            if (AppSettings.Instance.UseRemoteSearchDatabase) { return; }

            try {
                var migrations = _mediator.Query(new ListPossibleMigrationsQuery());
                foreach (var migration in migrations) {
                    _mediator.Command(new ApplyMigrationCommand {
                        Version = migration
                    });
                }
            } catch (Exception ex) { Logger.Error("ERROR ON UPDATE DATABASE: {0}", ex.Message); throw; }
        }

        #endregion IAction Members
    }
}