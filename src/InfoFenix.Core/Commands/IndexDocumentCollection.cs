using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using InfoFenix.Core.Cqrs;
using InfoFenix.Core.Data;
using InfoFenix.Core.Dto;
using InfoFenix.Core.Logging;
using InfoFenix.Core.PubSub;
using InfoFenix.Core.Search;
using InfoFenix.Core.Entities;
using System.Data;

namespace InfoFenix.Core.Commands {

    public class IndexDocumentCollectionCommand : ICommand {

        #region Public Properties

        public string IndexName { get; set; }

        public IList<DocumentIndexDto> Documents { get; set; } = new List<DocumentIndexDto>();

        public int BatchSize { get; set; }

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
            _publisherSubscriber.PublishAsync(new ProgressiveTaskStartNotification {
                Title = Resources.Resources.IndexDocumentCollection_ProgressiveTask_Title,
                TotalSteps = totalSteps
            });
        }

        private void PerformStepNotification(int actualStep, int totalSteps) {
            _publisherSubscriber.PublishAsync(new ProgressiveTaskPerformStepNotification {
                ActualStep = actualStep,
                TotalSteps = totalSteps
            });
        }

        private void CancelNotification(int totalSteps) {
            _publisherSubscriber.PublishAsync(new ProgressiveTaskCancelNotification {
                TotalSteps = totalSteps
            });
        }

        private void ErrorNotification(string error, int actualStep, int totalSteps) {
            _publisherSubscriber.PublishAsync(new ProgressiveTaskErrorNotification {
                ActualStep = actualStep,
                Error = error,
                TotalSteps = totalSteps
            });
        }

        private void CompleteNotification(int actualStep, int totalSteps, string error = null) {
            _publisherSubscriber.PublishAsync(new ProgressiveTaskCompleteNotification {
                ActualStep = actualStep,
                Error = error,
                TotalSteps = totalSteps
            });
        }

        private void Flush(IIndex index, int batchSize, IList<IDocumentIndex> documents, bool force = false) {
            if (documents.Count % batchSize == 0 || force) {
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

        #endregion


        #region ICommandHandler<IndexDocumentCollectionCommand> Members

        public Task HandleAsync(IndexDocumentCollectionCommand command, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => {
                var totalDocuments = command.Documents.Count;
                StartNotification(totalDocuments);

                var error = string.Empty;
                var step = 0;
                try {
                    // Gets the Lucene Index
                    var index = _indexProvider.GetOrCreate(command.IndexName);
                    var documentIndexList = new List<IDocumentIndex>();
                    for (var document = 0; document < totalDocuments; document++) {
                        // Exits the foreach loops if task is cancelled.
                        if (cancellationToken.IsCancellationRequested) {
                            // Before ends, notify about cancellation.
                            CancelNotification(step);

                            break;
                        }

                        documentIndexList.Add(command.Documents[document].Map());

                        Flush(index, command.BatchSize, documentIndexList);

                        PerformStepNotification(++step, totalDocuments);
                    }

                    // Flush to Lucene Index the last documents.
                    Flush(index, command.BatchSize, documentIndexList, force: true);
                } catch (Exception ex) {
                    ErrorNotification(ex.Message, step, totalDocuments);

                    error = ex.Message;

                    Log.Error(ex, ex.Message);

                    throw;
                } finally { CompleteNotification(step, totalDocuments, error); }
            }, cancellationToken);
        }

        #endregion ICommandHandler<IndexDocumentCollectionCommand> Members
    }
}