using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using InfoFenix.Core.Cqrs;
using InfoFenix.Core.Data;
using InfoFenix.Core.Dto;
using InfoFenix.Core.Logging;
using InfoFenix.Core.Search;
using Resource = InfoFenix.Core.Resources.Resources;

namespace InfoFenix.Core.Commands {

    public sealed class RemoveDocumentCommand : ICommand {

        #region Public Properties

        public DocumentDto Document { get; set; }

        #endregion Public Properties
    }

    public sealed class RemoveDocumentCommandHandler : ICommandHandler<RemoveDocumentCommand> {

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

        public RemoveDocumentCommandHandler(IDatabase database, IIndexProvider indexProvider) {
            Prevent.ParameterNull(database, nameof(database));
            Prevent.ParameterNull(indexProvider, nameof(indexProvider));

            _database = database;
            _indexProvider = indexProvider;
        }

        #endregion Public Constructors

        #region ICommandHandler<RemoveDocumentCommand> Members

        public Task HandleAsync(RemoveDocumentCommand command, CancellationToken cancellationToken = default(CancellationToken), IProgress<ProgressInfo> progress = null) {
            return Task.Run(() => {
                var actualStep = 0;
                var totalSteps = 2;

                try {
                    progress.Start(totalSteps, Resource.RemoveDocument_Progress_Start_Title);

                    using (var transaction = _database.Connection.BeginTransaction()) {
                        progress.PerformStep(++actualStep, totalSteps, Resource.RemoveDocument_Progress_Step_Database_Message, command.Document.FileName);

                        _database.ExecuteScalar(Resource.RemoveDocumentSQL, parameters: new[] {
                            Parameter.CreateInputParameter(Common.DatabaseSchema.Documents.DocumentID, command.Document.DocumentID, DbType.Int32)
                        });

                        if (cancellationToken.IsCancellationRequested) {
                            progress.Cancel(actualStep, totalSteps);

                            cancellationToken.ThrowIfCancellationRequested();
                        }
                        transaction.Commit();

                        progress.PerformStep(++actualStep, totalSteps, Resource.RemoveDocument_Progress_Step_Index_Message, command.Document.FileName);

                        _indexProvider
                            .GetOrCreate(command.Document.DocumentDirectory.Code)
                            .DeleteDocuments(command.Document.DocumentID.ToString());
                    }

                    progress.Complete();
                } catch (Exception ex) { progress.Error(actualStep, totalSteps, ex.Message); throw; }
            }, cancellationToken);
        }

        #endregion ICommandHandler<RemoveDocumentCommand> Members
    }
}