using System;
using System.IO;
using InfoFenix.Core.Commands;
using InfoFenix.Core.Cqrs;
using InfoFenix.Core.Logging;
using InfoFenix.Core.Office;
using InfoFenix.Core.PubSub;
using InfoFenix.Core.Queries;
using InfoFenix.Core.Search;

namespace InfoFenix.Core.Services {

    public class DocumentService : IDocumentService {

        #region Private Read-Only Fields

        private readonly ICommandQueryDispatcher _commandQueryDispatcher;
        private readonly IIndexProvider _indexProvider;
        private readonly IPublisherSubscriber _publisherSubscriber;
        private readonly IWordApplication _wordApplication;

        #endregion Private Read-Only Fields

        #region Public Properties

        private ILogger _log;
        public ILogger Log {
            get { return _log ?? NullLogger.Instance; }
            set { _log = value ?? NullLogger.Instance; }
        }

        #endregion

        #region Public Constructors

        public DocumentService(ICommandQueryDispatcher commandQueryDispatcher, IIndexProvider indexProvider, IPublisherSubscriber publisherSubscriber, IWordApplication wordApplication) {
            Prevent.ParameterNull(commandQueryDispatcher, nameof(commandQueryDispatcher));
            Prevent.ParameterNull(indexProvider, nameof(indexProvider));
            Prevent.ParameterNull(publisherSubscriber, nameof(publisherSubscriber));
            Prevent.ParameterNull(wordApplication, nameof(wordApplication));

            _commandQueryDispatcher = commandQueryDispatcher;
            _indexProvider = indexProvider;
            _publisherSubscriber = publisherSubscriber;
            _wordApplication = wordApplication;
        }

        #endregion Public Constructors

        #region IDocumentService Members

        public void Index(int documentID) {
            var document = _commandQueryDispatcher.Query(new GetDocumentQuery { ID = documentID });
            if (document == null) { return; }

            var documentDirectory = _commandQueryDispatcher.Query(new GetDocumentDirectoryQuery { ID = document.DocumentDirectoryID });
            var index = _indexProvider.GetOrCreate(documentDirectory.Code);

            string content = string.Empty;
            try {
                using (var memoryStream = new MemoryStream(document.Payload))
                using (var wordDocument = _wordApplication.Open(memoryStream)) {
                    content = wordDocument.Text;
                }
            } catch (Exception ex) { Log.Error(ex, $"ERROR INDEXING DOCUMENT: {document.Path}"); }

            var documentIndex = index.NewDocument(document.ID.ToString());
            documentIndex.Add(Common.Index.DocumentFieldName.Content, content).Analyze();
            documentIndex.Add(Common.Index.DocumentFieldName.DocumentDirectoryCode, documentDirectory.Code).Store();
            documentIndex.Add(Common.Index.DocumentFieldName.DocumentCode, document.Code).Store();
            index.StoreDocuments(documentIndex);

            _commandQueryDispatcher.Command(new SetDocumentIndexCommand { ID = document.ID });
        }

        public void Remove(int documentID) {
            _commandQueryDispatcher.Command(new RemoveDocumentCommand { ID = documentID });
        }

        public void Process(string documentDirectoryPath, string documentPath) {
            Prevent.ParameterNullOrWhiteSpace(documentDirectoryPath, nameof(documentDirectoryPath));
            Prevent.ParameterNullOrWhiteSpace(documentPath, nameof(documentPath));

            if (!Directory.Exists(documentDirectoryPath)) { return; }
            if (!File.Exists(documentPath)) { return; }

            var documentDirectory = _commandQueryDispatcher.Query(new GetDocumentDirectoryByPathQuery { Path = documentDirectoryPath });

            if (documentDirectory == null) { return; }

            _publisherSubscriber.PublishAsync(new ProcessDocumentInitializeNotification {
                FileName = Path.GetFileName(documentPath)
            });

            var saveDocumentCommand = new SaveDocumentCommand {
                DocumentDirectoryID = documentDirectory.ID,
                Path = documentPath,
                LastWriteTime = File.GetLastWriteTime(documentPath),
                Code = Common.ExtractCodeFromFilePath(documentPath),
                Indexed = false,
                Payload = File.ReadAllBytes(documentPath)
            };
            _commandQueryDispatcher.Command(saveDocumentCommand);

            Index(saveDocumentCommand.ID);

            _publisherSubscriber.PublishAsync(new ProcessDocumentCompleteNotification {
                FileName = Path.GetFileName(documentPath)
            });
        }

        #endregion IDocumentService Members
    }
}