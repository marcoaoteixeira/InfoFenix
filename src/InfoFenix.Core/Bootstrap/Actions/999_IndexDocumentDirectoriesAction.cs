using InfoFenix.Core.Cqrs;

namespace InfoFenix.Core.Bootstrap.Actions {

    [Order(999)]
    public class IndexDocumentDirectoriesAction : ActionBase {

        #region Private Read-Only Fields

        private readonly IMediator _mediator;

        #endregion Private Read-Only Fields

        #region Public Constructors

        public IndexDocumentDirectoriesAction(IMediator mediator) {
            Prevent.ParameterNull(mediator, nameof(mediator));

            _mediator = mediator;
        }

        #endregion Public Constructors

        #region IAction Members

        public override string Name => "Indexar Diretórios de Documentos";

        public override void Execute() {
        }

        #endregion IAction Members
    }
}