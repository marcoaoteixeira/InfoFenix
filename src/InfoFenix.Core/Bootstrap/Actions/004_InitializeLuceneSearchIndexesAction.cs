﻿using InfoFenix.Core.Search;
using InfoFenix.Core.Services;

namespace InfoFenix.Core.Bootstrap.Actions {

    [Order(4)]
    public class InitializeLuceneSearchIndexesAction : ActionBase {

        #region Private Read-Only Fields

        private readonly IDocumentDirectoryService _documentDirectoryService;
        private readonly IIndexProvider _indexProvider;

        #endregion Private Read-Only Fields

        #region Public Constructors

        public InitializeLuceneSearchIndexesAction(IDocumentDirectoryService documentDirectoryService, IIndexProvider indexProvider) {
            Prevent.ParameterNull(documentDirectoryService, nameof(documentDirectoryService));
            Prevent.ParameterNull(indexProvider, nameof(indexProvider));

            _documentDirectoryService = documentDirectoryService;
            _indexProvider = indexProvider;
        }

        #endregion Public Constructors

        #region IAction Members

        public override string Name => "Inicializar Motor de Pesquisa";

        public override void Execute() {
            var documentDirectories = _documentDirectoryService.List();
            foreach (var documentDirectory in documentDirectories) {
                _indexProvider.GetOrCreate(documentDirectory.Code);
            }
        }

        #endregion IAction Members
    }
}