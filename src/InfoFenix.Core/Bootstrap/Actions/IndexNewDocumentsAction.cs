using System.Linq;
using InfoFenix.Core.Cqrs;
using InfoFenix.Core.Documents;
using InfoFenix.Core.Queries;

namespace InfoFenix.Core.Bootstrap.Actions {

    [Order(4)]
    public class IndexNewDocumentsAction : ActionBase {

        #region Private Read-Only Fields

        private readonly IDispatcher _dispatcher;
        private readonly IDocumentProcessor _documentProcessor;
        private readonly IDocumentDirectoryProcessor _documentDirectoryProcessor;

        #endregion Private Read-Only Fields

        #region Public Constructors

        public IndexNewDocumentsAction(IDispatcher dispatcher, IDocumentProcessor documentProcessor, IDocumentDirectoryProcessor documentDirectoryProcessor) {
            Prevent.ParameterNull(dispatcher, nameof(dispatcher));
            Prevent.ParameterNull(documentProcessor, nameof(documentProcessor));
            Prevent.ParameterNull(documentDirectoryProcessor, nameof(documentDirectoryProcessor));

            _dispatcher = dispatcher;
            _documentProcessor = documentProcessor;
            _documentDirectoryProcessor = documentDirectoryProcessor;
        }

        #endregion Public Constructors

        #region IAction Members

        public override void Execute() {
            var documentDirectories = _dispatcher.Query(new ListDocumentDirectoriesQuery());
            foreach (var documentDirectory in documentDirectories) {
                var documents = _dispatcher.Query(new ListDocumentsByDocumentDirectoryQuery { DocumentDirectoryLabel = documentDirectory.Label });
                var indexedDocuments = documents.Select(_ => _.FilePath).ToArray();
                var files = Common.GetDocFiles(documentDirectory.DirectoryPath);
                var filesToIndex = files.Except(indexedDocuments);
                foreach (var fileToIndex in filesToIndex) {
                    _documentProcessor.Process(documentDirectory.Label, fileToIndex);
                }
            }
        }

        #endregion IAction Members
    }
}