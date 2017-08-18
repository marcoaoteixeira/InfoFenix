using System;
using InfoFenix.Migrations;
using InfoFenix.CQRS;
using InfoFenix.Logging;
using InfoFenix.Domains.Queries;

namespace InfoFenix.Bootstrap.Actions {

    [Order(3)]
    public class ExecuteDatabaseMigrationsAction : ActionBase {

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

        public ExecuteDatabaseMigrationsAction(IMediator mediator) {
            Prevent.ParameterNull(mediator, nameof(mediator));

            _mediator = mediator;
        }

        #endregion Public Constructors

        #region IAction Members

        public override string Name => "Atualizando Banco de Dados";

        public override void Execute() {
            if (AppSettings.Instance.UseRemoteSearchDatabase) { return; }

            try {
                var migrationsExists = _mediator.Query(new ObjectExistsQuery {
                    Type = ObjectType.Table,
                    Name = Common.DatabaseSchema.Migrations.TableName
                });

                if (!migrationsExists) {
                    _mediator.Command(new CreateMigrationTableSchemaCommand());
                }

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