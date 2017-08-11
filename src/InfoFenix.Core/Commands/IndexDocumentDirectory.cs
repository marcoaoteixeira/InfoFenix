using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using InfoFenix.Core.CQRS;
using InfoFenix.Core.Data;
using InfoFenix.Core.Entities;
using InfoFenix.Core.Logging;
using InfoFenix.Core.Search;
using Resource = InfoFenix.Core.Resources.Resources;

namespace InfoFenix.Core.Commands {

    public class IndexDocumentDirectoryCommand : ICommand {

        #region Public Properties

        public int DocumentDirectoryID { get; set; }

        public int BatchSize { get; set; } = 128;

        #endregion Public Properties
    }

    public class IndexDocumentDirectoryCommandHandler : ICommandHandler<IndexDocumentDirectoryCommand> {

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

        public IndexDocumentDirectoryCommandHandler(IDatabase database, IIndexProvider indexProvider) {
            Prevent.ParameterNull(database, nameof(database));
            Prevent.ParameterNull(indexProvider, nameof(indexProvider));

            _database = database;
            _indexProvider = indexProvider;
        }

        #endregion Public Constructors

        #region Private Methods

        private void MarkAsIndex(IEnumerable<Document> documents) {
            using (var transaction = _database.Connection.BeginTransaction()) {
                foreach (var document in documents) {
                    _database.ExecuteNonQuery(Resource.SetDocumentIndexSQL, parameters: new[] {
                        Parameter.CreateInputParameter(Common.DatabaseSchema.Documents.DocumentID, document.DocumentID, DbType.Int32)
                    });
                }
                transaction.Commit();
            }
        }

        #endregion Private Methods

        #region ICommandHandler<IndexDocumentDirectoryCommand> Members

        public Task HandleAsync(IndexDocumentDirectoryCommand command, CancellationToken cancellationToken = default(CancellationToken), IProgress<ProgressInfo> progress = null) {
            return Task.Run(() => {
                var batchSize = command.BatchSize;
                var documentDirectory = _database.ExecuteReaderSingle(Resource.GetDocumentDirectorySQL, DocumentDirectory.Map, parameters: new[] {
                    Parameter.CreateInputParameter(Common.DatabaseSchema.DocumentDirectories.DocumentDirectoryID, command.DocumentDirectoryID, DbType.Int32)
                });
                var documentCount = (long)_database.ExecuteScalar(Resource.GetDocumentCountByDocumentDirectorySQL, parameters: new[] {
                    Parameter.CreateInputParameter(Common.DatabaseSchema.DocumentDirectories.DocumentDirectoryID, command.DocumentDirectoryID, DbType.Int32)
                });

                var actualStep = 0;
                var totalSteps = Convert.ToInt32(documentCount);

                try {
                    progress.Start(totalSteps, Resource.IndexDocumentDirectory_Progress_Start_Title);

                    // Gets the Lucene Index
                    var index = _indexProvider.GetOrCreate(documentDirectory.Code);
                    var documentIndexList = new List<IDocumentIndex>();
                    var pageCount = (documentCount / batchSize) + 1;
                    for (var page = 0; page < pageCount; page++) {
                        var documents = _database.ExecuteReader(Resource.PaginateDocumentsByDocumentDirectorySQL, Document.Map, parameters: new[] {
                            Parameter.CreateInputParameter(Common.DatabaseSchema.DocumentDirectories.DocumentDirectoryID, command.DocumentDirectoryID, DbType.Int32),
                            Parameter.CreateInputParameter(Common.DatabaseSchema.Documents.Index, false /* true */, DbType.Int32),
                            Parameter.CreateInputParameter("skip", page * batchSize, DbType.Int32),
                            Parameter.CreateInputParameter("count", batchSize, DbType.Int32)
                        });

                        foreach (var document in documents) {
                            progress.PerformStep(++actualStep, totalSteps, Resource.IndexDocumentDirectory_Progress_Step_Message, document.FileName);
                            if (cancellationToken.IsCancellationRequested) {
                                progress.Cancel(actualStep, totalSteps);

                                cancellationToken.ThrowIfCancellationRequested();
                            }
                            var documentIndexDto = new DocumentIndexDto {
                                DocumentID = document.DocumentID,
                                DocumentCode = document.Code,
                                DocumentDirectoryCode = documentDirectory.Code,
                                Content = document.Content,
                                FileName = document.FileName
                            };
                            documentIndexList.Add(documentIndexDto.Map());
                        }
                        index.StoreDocuments(documentIndexList.ToArray());
                        documentIndexList.Clear();
                        MarkAsIndex(documents);
                    }

                    progress.Complete(actualStep, totalSteps);
                } catch (Exception ex) { progress.Error(actualStep, totalSteps, ex.Message); throw; }
            }, cancellationToken);
        }

        #endregion ICommandHandler<IndexDocumentDirectoryCommand> Members
    }
}