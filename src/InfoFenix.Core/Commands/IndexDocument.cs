using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using InfoFenix.Core.Cqrs;
using InfoFenix.Core.Data;
using InfoFenix.Core.Dto;
using InfoFenix.Core.Search;
using Resource = InfoFenix.Core.Resources.Resources;

namespace InfoFenix.Core.Commands {

    public class IndexDocumentCommand : ICommand {

        #region Public Properties

        public string IndexName { get; set; }

        public DocumentIndexDto DocumentIndex { get; set; }

        #endregion Public Properties
    }

    public class IndexDocumentCommandHandler : ICommandHandler<IndexDocumentCommand> {

        #region Private Read-Only Fields

        private IDatabase _database;
        private IIndexProvider _indexProvider;

        #endregion Private Read-Only Fields

        #region Public Constructors

        public IndexDocumentCommandHandler(IDatabase database, IIndexProvider indexProvider) {
            Prevent.ParameterNull(database, nameof(database));
            Prevent.ParameterNull(indexProvider, nameof(indexProvider));

            _database = database;
            _indexProvider = indexProvider;
        }

        #endregion Public Constructors

        #region ICommandHandler<IndexDocumentCommand> Members

        public Task HandleAsync(IndexDocumentCommand command, CancellationToken cancellationToken = default(CancellationToken), IProgress<ProgressInfo> progress = null) {
            return Task.Run(() => {
                var actualStep = 0;
                var totalSteps = 2;

                try {
                    progress.Start(totalSteps, Resource.IndexDocument_Progress_Start_Title);

                    // First Step: Write index
                    var index = _indexProvider.GetOrCreate(command.IndexName);
                    progress.PerformStep(++actualStep, totalSteps, Resource.IndexDocument_Progress_Step_Index_Message, command.DocumentIndex.FileName);
                    if (cancellationToken.IsCancellationRequested) {
                        progress.Cancel(actualStep, totalSteps);

                        cancellationToken.ThrowIfCancellationRequested();
                    }

                    index.StoreDocuments(command.DocumentIndex.Map());

                    // Second Step: Update database
                    progress.PerformStep(++actualStep, totalSteps, Resource.IndexDocument_Progress_Step_Database_Message, command.DocumentIndex.FileName);
                    if (cancellationToken.IsCancellationRequested) {
                        progress.Cancel(actualStep, totalSteps);

                        cancellationToken.ThrowIfCancellationRequested();
                    }
                    using (var transaction = _database.Connection.BeginTransaction()) {
                        _database.ExecuteNonQuery(Resource.SetDocumentIndexSQL, parameters: new[] {
                            Parameter.CreateInputParameter(Common.DatabaseSchema.Documents.DocumentID, command.DocumentIndex.DocumentID, DbType.Int32)
                        });
                        transaction.Commit();
                    }

                    progress.Complete(actualStep, totalSteps);
                } catch (Exception ex) { progress.Error(actualStep, totalSteps, ex.Message); throw; }
            });
        }

        #endregion ICommandHandler<IndexDocumentCommand> Members
    }
}