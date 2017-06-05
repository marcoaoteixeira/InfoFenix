using InfoFenix.Core.Cqrs;
using InfoFenix.Core.Queries;
using InfoFenix.Core.Search;

namespace InfoFenix.Core.Bootstrap.Actions {

    [Order(4)]
    public class InitializeLuceneSearchIndexesAction : ActionBase {

        #region Private Read-Only Fields

        private readonly ICommandQueryDispatcher _commandQueryDispatcher;
        private readonly IIndexProvider _indexProvider;

        #endregion Private Read-Only Fields

        #region Public Constructors

        public InitializeLuceneSearchIndexesAction(ICommandQueryDispatcher commandQueryDispatcher, IIndexProvider indexProvider) {
            Prevent.ParameterNull(commandQueryDispatcher, nameof(commandQueryDispatcher));
            Prevent.ParameterNull(indexProvider, nameof(indexProvider));

            _commandQueryDispatcher = commandQueryDispatcher;
            _indexProvider = indexProvider;
        }

        #endregion Public Constructors

        #region IAction Members

        public override string Description => "Inicializando motor de pesquisa do aplicativo...";

        public override void Execute() {
            var documentDirectories = _commandQueryDispatcher.Query(new ListDocumentDirectoriesQuery());
            foreach (var documentDirectory in documentDirectories) {
                _indexProvider.GetOrCreate(documentDirectory.Code);
            }
        }

        #endregion IAction Members
    }
}