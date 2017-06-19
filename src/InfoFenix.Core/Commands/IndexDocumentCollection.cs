using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InfoFenix.Core.Cqrs;
using InfoFenix.Core.Data;
using InfoFenix.Core.Dto;
using InfoFenix.Core.Logging;
using InfoFenix.Core.PubSub;
using InfoFenix.Core.Search;
using Resource = InfoFenix.Core.Resources.Resources;

namespace InfoFenix.Core.Commands {

    public class IndexDocumentCollectionCommand : ICommand {

        #region Public Properties

        public string IndexName { get; set; }

        public IList<DocumentIndexDto> Documents { get; set; } = new List<DocumentIndexDto>();

        public int BatchSize { get; set; } = 128;

        #endregion Public Properties
    }

    public class IndexDocumentCollectionCommandHandler : ICommandHandler<IndexDocumentCollectionCommand> {

        #region Private Read-Only Fields

        private readonly IDatabase _database;
        private readonly IIndexProvider _indexProvider;
        private readonly IPublisherSubscriber _publisherSubscriber;

        #endregion Private Read-Only Fields

        #region Public Properties

        private ILogger _log;

        public ILogger Log {
            get { return _log ?? NullLogger.Instance; }
            set { _log = value ?? NullLogger.Instance; }
        }

        #endregion Public Properties

        #region Public Constructors

        public IndexDocumentCollectionCommandHandler(IDatabase database, IIndexProvider indexProvider, IPublisherSubscriber publisherSubscriber) {
            Prevent.ParameterNull(database, nameof(database));
            Prevent.ParameterNull(indexProvider, nameof(indexProvider));
            Prevent.ParameterNull(publisherSubscriber, nameof(publisherSubscriber));

            _database = database;
            _indexProvider = indexProvider;
            _publisherSubscriber = publisherSubscriber;
        }

        #endregion Public Constructors

        #region Private Methods

        private void Flush(IIndex index, int batchSize, int step, IList<IDocumentIndex> documents, bool force = false) {
            if (step % batchSize == 0 || force) {
                index.StoreDocuments(documents.ToArray());

                MarkAsIndexed(documents);

                documents.Clear();
            }
        }

        private void MarkAsIndexed(IEnumerable<IDocumentIndex> documents) {
            using (var transaction = _database.Connection.BeginTransaction()) {
                foreach (var document in documents) {
                    _database.ExecuteNonQuery(Resource.SetDocumentIndexSQL, parameters: new[] {
                        Parameter.CreateInputParameter(Common.DatabaseSchema.Documents.DocumentID, int.Parse(document.DocumentID), DbType.Int32)
                    });
                }
                transaction.Commit();
            }
        }

        #endregion Private Methods

        #region ICommandHandler<IndexDocumentCollectionCommand> Members

        public Task HandleAsync(IndexDocumentCollectionCommand command, CancellationToken cancellationToken = default(CancellationToken)) {
            var info = new ProgressiveTaskContinuationInfo {
                Log = Log,
                TotalSteps = command.Documents.Count
            };

            return Task.Run(() => {
                _publisherSubscriber.ProgressiveTaskStartAsync(
                    title: Resource.IndexDocumentCollection_ProgressiveTaskStart_Title,
                    actualStep: info.ActualStep,
                    totalSteps: info.TotalSteps
                );

                // Gets the Lucene Index
                var index = _indexProvider.GetOrCreate(command.IndexName);
                var documentIndexList = new List<IDocumentIndex>();
                foreach (var document in command.Documents) {
                    cancellationToken.ThrowIfCancellationRequested();

                    _publisherSubscriber.ProgressiveTaskPerformStepAsync(
                        message: string.Format(Resource.IndexDocumentCollection_ProgressiveTaskPerformStep_Message, document.FileName),
                        actualStep: ++info.ActualStep,
                        totalSteps: info.TotalSteps
                    );

                    documentIndexList.Add(document.Map());

                    Flush(index, command.BatchSize, info.ActualStep, documentIndexList);
                }

                // Flush to Lucene Index the last documents.
                Flush(index, command.BatchSize, info.ActualStep, documentIndexList, force: true);
            }, cancellationToken)
            .ContinueWith(_publisherSubscriber.TaskContinuation, info);
        }

        #endregion ICommandHandler<IndexDocumentCollectionCommand> Members
    }
}