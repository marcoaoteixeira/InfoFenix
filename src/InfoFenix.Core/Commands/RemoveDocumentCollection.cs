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

    public sealed class RemoveDocumentCollectionCommand : ICommand {

        #region Public Properties

        public DocumentDirectoryDto DocumentDirectory { get; set; }

        public IList<DocumentDto> Documents { get; set; } = new List<DocumentDto>();

        #endregion Public Properties
    }

    public sealed class RemoveDocumentCollectionCommandHandler : ICommandHandler<RemoveDocumentCollectionCommand> {

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

        public RemoveDocumentCollectionCommandHandler(IDatabase database, IIndexProvider indexProvider) {
            Prevent.ParameterNull(database, nameof(database));
            Prevent.ParameterNull(indexProvider, nameof(indexProvider));

            _database = database;
            _indexProvider = indexProvider;
        }

        #endregion Public Constructors

        #region ICommandHandler<RemoveDocumentCollectionCommand> Members

        public Task HandleAsync(RemoveDocumentCollectionCommand command, CancellationToken cancellationToken = default(CancellationToken), IProgress<ProgressInfo> progress = null) {
            return Task.Run(() => {
                var documentIDs = command.Documents.Select(_ => _.DocumentID).ToArray();
                var actualStep = 0;
                var totalSteps = (documentIDs.Length + 1);

                try {
                    progress.Start(totalSteps, Resource.RemoveDocumentCollection_Progress_Start_Title);

                    using (var transaction = _database.Connection.BeginTransaction()) {
                        foreach (var document in command.Documents) {
                            progress.PerformStep(++actualStep, totalSteps, Resource.RemoveDocumentCollection_Progress_Step_Database_Message, document.FileName);

                            if (cancellationToken.IsCancellationRequested) {
                                progress.Cancel(actualStep, totalSteps);

                                cancellationToken.ThrowIfCancellationRequested();
                            }

                            _database.ExecuteScalar(Resource.RemoveDocumentSQL, parameters: new[] {
                                Parameter.CreateInputParameter(Common.DatabaseSchema.Documents.DocumentID, document.DocumentID, DbType.Int32)
                            });
                        }

                        if (cancellationToken.IsCancellationRequested) {
                            progress.Cancel(actualStep, totalSteps);

                            cancellationToken.ThrowIfCancellationRequested();
                        }
                        transaction.Commit();

                        progress.PerformStep(++actualStep, totalSteps, Resource.RemoveDocumentCollection_Progress_Step_Index_Message, command.DocumentDirectory.Label);

                        _indexProvider
                            .GetOrCreate(command.DocumentDirectory.Code)
                            .DeleteDocuments(command.Documents.Select(_ => _.DocumentID.ToString()).ToArray());
                    }

                    progress.Complete(actualStep, totalSteps);
                } catch (Exception ex) { progress.Error(actualStep, totalSteps, ex.Message); throw; }
            }, cancellationToken);
        }

        #endregion ICommandHandler<RemoveDocumentCollectionCommand> Members
    }
}