using InfoFenix.CQRS;
using InfoFenix.Domains.Queries;
using InfoFenix.Search;

namespace InfoFenix.Bootstrap.Actions {

    [Order(5)]
    public class InitializeLuceneSearchIndexesAction : ActionBase {

        #region Private Read-Only Fields

        private readonly IIndexProvider _indexProvider;
        private readonly IMediator _mediator;

        #endregion Private Read-Only Fields

        #region Public Constructors

        public InitializeLuceneSearchIndexesAction(IIndexProvider indexProvider, IMediator mediator) {
            Prevent.ParameterNull(indexProvider, nameof(indexProvider));
            Prevent.ParameterNull(mediator, nameof(mediator));

            _indexProvider = indexProvider;
            _mediator = mediator;
        }

        #endregion Public Constructors

        #region IAction Members

        public override string Name => "Inicializar Motor de Pesquisa";

        public override void Execute() {
            _mediator
                .Query(new ListDocumentDirectoriesQuery())
                .Each(_ => _indexProvider.GetOrCreate(_.Code));
        }

        #endregion IAction Members
    }
}