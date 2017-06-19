using InfoFenix.Core.Commands;
using InfoFenix.Core.Cqrs;
using InfoFenix.Core.Queries;

namespace InfoFenix.Core.Bootstrap.Actions {

    [Order(6)]
    public class WatchDocumentDirectoriesAction : ActionBase {

        #region Private Read-Only Fields

        private readonly IMediator _mediator;

        #endregion Private Read-Only Fields

        #region Public Constructors

        public WatchDocumentDirectoriesAction(IMediator mediator) {
            Prevent.ParameterNull(mediator, nameof(mediator));

            _mediator = mediator;
        }

        #endregion Public Constructors

        #region IAction Members

        public override string Name => "Observar Diretórios de Documentos";

        public override void Execute() {
            _mediator
                .Query(new ListDocumentDirectoriesQuery())
                .Each(_ => _mediator.Command(new StartWatchDocumentDirectoryCommand {
                    DirectoryPath = _.Path
                }));
        }

        #endregion IAction Members
    }
}