using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InfoFenix.Core.Cqrs;
using InfoFenix.Core.Data;
using InfoFenix.Core.Dto;
using InfoFenix.Core.Logging;
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

        #endregion Private Read-Only Fields

        #region Public Properties

        private ILogger _log;

        public ILogger Log {
            get { return _log ?? NullLogger.Instance; }
            set { _log = value ?? NullLogger.Instance; }
        }

        #endregion Public Properties

        #region Public Constructors

        public IndexDocumentCollectionCommandHandler(IDatabase database, IIndexProvider indexProvider) {
            Prevent.ParameterNull(database, nameof(database));
            Prevent.ParameterNull(indexProvider, nameof(indexProvider));

            _database = database;
            _indexProvider = indexProvider;
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

        public Task HandleAsync(IndexDocumentCollectionCommand command, CancellationToken cancellationToken = default(CancellationToken), IProgress<ProgressInfo> progress = null) {
            return Task.Run(() => {
                var actualStep = 0;
                var totalSteps = command.Documents.Count;

                try {
                    progress.Start(totalSteps, Resource.IndexDocumentCollection_Progress_Start_Title);

                    // Gets the Lucene Index
                    var index = _indexProvider.GetOrCreate(command.IndexName);
                    var documentIndexList = new List<IDocumentIndex>();
                    foreach (var document in command.Documents) {
                        progress.PerformStep(++actualStep, totalSteps, Resource.IndexDocumentCollection_Progress_Step_Message, document.FileName);

                        if (cancellationToken.IsCancellationRequested) {
                            progress.Cancel(actualStep, totalSteps);

                            cancellationToken.ThrowIfCancellationRequested();
                        }

                        documentIndexList.Add(document.Map());

                        Flush(index, command.BatchSize, actualStep, documentIndexList);
                    }

                    if (documentIndexList.Count > 0) {
                        // Flush to Lucene Index the last documents.
                        Flush(index, command.BatchSize, actualStep, documentIndexList, force: true);
                    }

                    progress.Complete(actualStep, totalSteps);
                } catch (Exception ex) { progress.Error(actualStep, totalSteps, ex.Message); throw; }
            }, cancellationToken);
        }

        #endregion ICommandHandler<IndexDocumentCollectionCommand> Members
    }
}