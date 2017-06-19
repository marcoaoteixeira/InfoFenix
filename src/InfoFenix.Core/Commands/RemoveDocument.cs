using System.Data;
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

    public sealed class RemoveDocumentCommand : ICommand {

        #region Public Properties

        public DocumentDto Document { get; set; }

        #endregion Public Properties
    }

    public sealed class RemoveDocumentCommandHandler : ICommandHandler<RemoveDocumentCommand> {

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

        public RemoveDocumentCommandHandler(IDatabase database, IIndexProvider indexProvider, IPublisherSubscriber publisherSubscriber) {
            Prevent.ParameterNull(database, nameof(database));
            Prevent.ParameterNull(indexProvider, nameof(indexProvider));
            Prevent.ParameterNull(publisherSubscriber, nameof(publisherSubscriber));

            _database = database;
            _indexProvider = indexProvider;
            _publisherSubscriber = publisherSubscriber;
        }

        #endregion Public Constructors

        #region ICommandHandler<RemoveDocumentCommand> Members

        public Task HandleAsync(RemoveDocumentCommand command, CancellationToken cancellationToken = default(CancellationToken)) {
            var info = new ProgressiveTaskContinuationInfo {
                Log = Log,
                TotalSteps = 2
            };

            return Task.Run(() => {
                _publisherSubscriber.ProgressiveTaskStartAsync(
                    message: Resource.RemoveDocument_ProgressiveTaskStart_Title,
                    actualStep: info.ActualStep,
                    totalSteps: info.TotalSteps,
                    log: Log
                );

                using (var transaction = _database.Connection.BeginTransaction()) {
                    _publisherSubscriber.ProgressiveTaskPerformStepAsync(
                        message: string.Format(Resource.RemoveDocument_ProgressiveTaskPerformStep_Database_Message, command.Document.FileName),
                        actualStep: ++info.ActualStep,
                        totalSteps: info.TotalSteps,
                        log: Log
                    );

                    _database.ExecuteScalar(Resource.RemoveDocumentSQL, parameters: new[] {
                        Parameter.CreateInputParameter(Common.DatabaseSchema.Documents.DocumentID, command.Document.DocumentID, DbType.Int32)
                    });

                    cancellationToken.ThrowIfCancellationRequested();
                    transaction.Commit();

                    _publisherSubscriber.ProgressiveTaskPerformStepAsync(
                        message: string.Format(Resource.RemoveDocument_ProgressiveTaskPerformStep_Index_Message, command.Document.FileName),
                        actualStep: ++info.ActualStep,
                        totalSteps: info.TotalSteps,
                        log: Log
                    );

                    _indexProvider
                        .GetOrCreate(command.Document.DocumentDirectory.Code)
                        .DeleteDocuments(command.Document.DocumentID.ToString());
                }
            }, cancellationToken)
            .ContinueWith(_publisherSubscriber.TaskContinuation, info);
        }

        #endregion ICommandHandler<RemoveDocumentCommand> Members
    }
}