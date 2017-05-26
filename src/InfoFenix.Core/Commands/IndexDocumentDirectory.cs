using System.Data;
using System.IO;
using System.Linq;
using InfoFenix.Core.Cqrs;
using InfoFenix.Core.Data;
using InfoFenix.Core.Entities;
using InfoFenix.Core.Office;
using InfoFenix.Core.PubSub;
using InfoFenix.Core.Search;
using SQL = InfoFenix.Core.Resources.Resources;

namespace InfoFenix.Core.Commands {

    public class IndexDocumentDirectoryCommand : ICommand {

        #region Public Properties

        public int DocumentDirectoryID { get; set; }

        public string DocumentDirectoryCode { get; set; }

        public string DocumentDirectoryPath { get; set; }

        #endregion Public Properties
    }

    public class IndexDocumentDirectoryCommandHandler : ICommandHandler<IndexDocumentDirectoryCommand> {

        #region Private Read-Only Fields

        private readonly IDatabase _database;
        private readonly IIndexProvider _indexProvider;
        private readonly IPublisherSubscriber _publisherSubscriber;
        private readonly IWordDocumentService _wordDocumentService;

        #endregion Private Read-Only Fields

        #region Public Constructors

        public IndexDocumentDirectoryCommandHandler(IDatabase database, IIndexProvider indexProvider, IPublisherSubscriber publisherSubscriber, IWordDocumentService wordDocumentService) {
            Prevent.ParameterNull(database, nameof(database));
            Prevent.ParameterNull(indexProvider, nameof(indexProvider));
            Prevent.ParameterNull(publisherSubscriber, nameof(publisherSubscriber));
            Prevent.ParameterNull(wordDocumentService, nameof(wordDocumentService));

            _database = database;
            _indexProvider = indexProvider;
            _publisherSubscriber = publisherSubscriber;
            _wordDocumentService = wordDocumentService;
        }

        #endregion Public Constructors

        #region Private Methods

        private string CleanContent(string content) {
            return content;
        }

        #endregion Private Methods

        #region ICommandHandler<IndexDocumentDirectoryCommand> Members

        public void Handle(IndexDocumentDirectoryCommand command) {
            var documents = _database.ExecuteReader(SQL.ListDocumentsByDocumentDirectory, DocumentEntity.MapFromDataReader, parameters: new[] {
                Parameter.CreateInputParameter(nameof(command.DocumentDirectoryID), command.DocumentDirectoryID, DbType.Int32)
            }).ToArray();
            var totalDocuments = documents.Length;
            var error = string.Empty;

            _publisherSubscriber.PublishAsync(new StartingDocumentDirectoryIndexingNotification {
                FullPath = command.DocumentDirectoryPath,
                TotalDocuments = totalDocuments
            });
            if (totalDocuments > 0) {
                var index = _indexProvider.GetOrCreate(command.DocumentDirectoryCode);
                foreach (var document in documents) {
                    _publisherSubscriber.PublishAsync(new StartingDocumentIndexingNotification {
                        FullPath = document.Path,
                        TotalDocuments = totalDocuments
                    });

                    var content = string.Empty;
                    using (var memoryStream = new MemoryStream(document.Payload))
                    using (var wordDocument = _wordDocumentService.Open(memoryStream)) {
                        content = CleanContent(wordDocument.Text);
                    }
                    var documentIndex = index.NewDocument(document.ID.ToString());
                    documentIndex.Add(Common.Index.Content, content).Analyze();
                    documentIndex.Add(Common.Index.DocumentDirectoryCode, command.DocumentDirectoryCode).Store();
                    documentIndex.Add(Common.Index.DocumentCode, document.Code).Store();
                    index.StoreDocuments(documentIndex);

                    _database.ExecuteNonQuery(SQL.SetDocumentIndexed, parameters: new[] {
                        Parameter.CreateInputParameter(nameof(document.ID), document.ID,  DbType.Int32)
                    });

                    _publisherSubscriber.PublishAsync(new DocumentIndexingCompleteNotification {
                        FullPath = document.Path
                    });
                }
            }
            _publisherSubscriber.PublishAsync(new DocumentDirectoryIndexingCompleteNotification {
                FullPath = command.DocumentDirectoryPath,
                TotalDocuments = totalDocuments
            });
        }

        #endregion ICommandHandler<IndexDocumentDirectoryCommand> Members
    }
}