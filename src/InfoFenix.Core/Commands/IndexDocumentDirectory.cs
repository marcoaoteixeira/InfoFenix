using System.Data;
using System.Linq;
using System.Threading.Tasks;
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

        public string Code { get; set; }

        public string DirectoryPath { get; set; }

        #endregion Public Properties
    }

    public class IndexDocumentDirectoryCommandHandler : ICommandHandler<IndexDocumentDirectoryCommand> {

        #region Private Read-Only Fields

        private readonly CancellationTokenIssuer _issuer;
        private readonly IDatabase _database;
        private readonly IIndexProvider _indexProvider;
        private readonly IMicrosoftWordApplication _application;
        private readonly IPublisherSubscriber _pubSub;

        #endregion Private Read-Only Fields

        #region Public Constructors

        public IndexDocumentDirectoryCommandHandler(CancellationTokenIssuer issuer, IDatabase database, IIndexProvider indexProvider, IMicrosoftWordApplication application, IPublisherSubscriber pubSub) {
            Prevent.ParameterNull(issuer, nameof(issuer));
            Prevent.ParameterNull(database, nameof(database));
            Prevent.ParameterNull(indexProvider, nameof(indexProvider));
            Prevent.ParameterNull(application, nameof(application));
            Prevent.ParameterNull(pubSub, nameof(pubSub));

            _issuer = issuer;
            _database = database;
            _indexProvider = indexProvider;
            _application = application;
            _pubSub = pubSub;
        }

        #endregion Public Constructors

        #region Private Methods

        private void ProcessDocuments(int documentDirectoryID, string directoryPath, string code) {
            var documents = _database.ExecuteReader(SQL.ListDocumentsByDocumentDirectory, DocumentEntity.MapFromDataReader, parameters: new[] {
                Parameter.CreateInputParameter(nameof(DocumentDirectoryEntity.DocumentDirectoryID), documentDirectoryID, DbType.Int32)
            }).ToArray();
            var totalDocuments = documents.Length;
            var error = string.Empty;

            _pubSub.PublishAsync(new StartingDocumentDirectoryIndexingNotification {
                FullPath = directoryPath,
                TotalDocuments = totalDocuments
            });
            if (totalDocuments > 0) {
                var index = _indexProvider.GetOrCreate(code);
                foreach (var document in documents) {
                    _pubSub.PublishAsync(new StartingDocumentIndexingNotification {
                        FullPath = document.FullPath,
                        TotalDocuments = totalDocuments
                    });

                    var wordDocument = _application.Open(document.FullPath);
                    var content = wordDocument.ReadContent();
                    wordDocument.Close();

                    var documentIndex = index.NewDocument(document.DocumentID.ToString());
                    documentIndex.Add(Common.DocumentIndex.Content, content).Analyze();
                    documentIndex.Add(Common.DocumentIndex.DocumentDirectoryCode, code).Store();
                    documentIndex.Add(Common.DocumentIndex.DocumentCode, document.Code).Store();
                    index.StoreDocuments(documentIndex);

                    _database.ExecuteNonQuery(SQL.SetDocumentIndexed, parameters: new[] {
                        Parameter.CreateInputParameter(nameof(DocumentEntity.DocumentID), document.DocumentID,  DbType.Int32)
                    });

                    _pubSub.PublishAsync(new DocumentIndexingCompleteNotification {
                        FullPath = document.FullPath
                    });
                }
            }
            _pubSub.PublishAsync(new DocumentDirectoryIndexingCompleteNotification {
                FullPath = directoryPath,
                TotalDocuments = totalDocuments
            });
        }

        #endregion Private Methods

        #region ICommandHandler<IndexDocumentDirectoryCommand> Members

        public void Handle(IndexDocumentDirectoryCommand command) {
            var key = $"INDEXING::{command.Code}";
            var token = _issuer.Get(key);
            Task
                .Run(() => ProcessDocuments(command.DocumentDirectoryID, command.DirectoryPath, command.Code), token)
                .ContinueWith(continuationAction => _issuer.MarkAsComplete(key));
        }

        #endregion ICommandHandler<IndexDocumentDirectoryCommand> Members
    }
}