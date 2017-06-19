using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InfoFenix.Core.Cqrs;
using InfoFenix.Core.Data;
using InfoFenix.Core.Dto;
using InfoFenix.Core.Entities;
using InfoFenix.Core.Logging;
using InfoFenix.Core.PubSub;
using InfoFenix.Core.Search;

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

        private void StartNotification(int totalSteps) {
            
        }

        private void PerformStepNotification(int actualStep, int totalSteps) {
            _publisherSubscriber.ProgressiveTaskPerformStepAsync(
                actualStep: actualStep,
                totalSteps: totalSteps
            );
        }

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
                    _database.ExecuteNonQuery(Resources.Resources.SetDocumentIndex, parameters: new[] {
                        Parameter.CreateInputParameter(nameof(DocumentEntity.ID), int.Parse(document.DocumentID), DbType.Int32)
                    });
                }

                transaction.Commit();
            }
        }

        #endregion Private Methods

        #region ICommandHandler<IndexDocumentCollectionCommand> Members

        public Task HandleAsync(IndexDocumentCollectionCommand command, CancellationToken cancellationToken = default(CancellationToken)) {
            var actualStep = 0;
            var totalDocuments = command.Documents.Count;

            return Task.Run(() => {
                _publisherSubscriber.ProgressiveTaskStartAsync(
                    title: Resources.Resources.IndexDocumentCollection_ProgressiveTaskStart_Title,
                    actualStep: actualStep,
                    totalSteps: totalDocuments
                );

                // Gets the Lucene Index
                var index = _indexProvider.GetOrCreate(command.IndexName);
                var documentIndexList = new List<IDocumentIndex>();
                foreach (var document in command.Documents) {
                    _publisherSubscriber.ProgressiveTaskPerformStepAsync(
                        message: string.Format(Resources.Resources.IndexDocumentCollection_ProgressiveTaskPerformStep_Message, document.FileName),
                        actualStep: ++actualStep,
                        totalSteps: totalDocuments
                    );

                    documentIndexList.Add(document.Map());

                    Flush(index, command.BatchSize, actualStep, documentIndexList);

                    // Exits the foreach loops if task is cancelled.
                    if (cancellationToken.IsCancellationRequested) { break; }
                }

                // Flush to Lucene Index the last documents.
                Flush(index, command.BatchSize, actualStep, documentIndexList, force: true);
            }, cancellationToken)
            .ContinueWith(_publisherSubscriber.TaskContinuation, new ProgressiveTaskContinuationInfo {
                ActualStep = actualStep,
                TotalSteps = totalDocuments,
                Log = Log
            });
        }

        #endregion ICommandHandler<IndexDocumentCollectionCommand> Members
    }
}