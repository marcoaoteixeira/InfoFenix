using System.Threading;
using InfoFenix.Core.Cqrs;
using InfoFenix.Core.Logging;
using InfoFenix.Core.Queries;
using InfoFenix.Core.Services;

namespace InfoFenix.Core.Bootstrap.Actions {

    [Order(5)]
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
            _mediator
                .Query(new ListDocumentDirectoriesQuery())
                .Each(_ => {
                    _mediator.CleanAsync(_.ID, CancellationToken.None);
                    _mediator.SaveDocumentsInsideDocumentDirectoryAsync(documentDirectory.ID, CancellationToken.None);
                    _mediator.IndexAsync(_.ID, CancellationToken.None);
                });
        }

        #endregion IAction Members
    }
}