using System.Data;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using InfoFenix.Core.Cqrs;
using InfoFenix.Core.Data;
using InfoFenix.Core.Dto;
using InfoFenix.Core.Entities;
using InfoFenix.Core.Logging;
using InfoFenix.Core.PubSub;
using InfoFenix.Core.Search;
using SQL = InfoFenix.Core.Resources.Resources;

using Strings = InfoFenix.Core.Resources.Resources;

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
            var actualStep = 0;
            var totalSteps = 2;

            return Task.Run(() => {
                _publisherSubscriber.ProgressiveTaskStartAsync(
                    message: Strings.RemoveDocument_ProgressiveTaskStart_Title,
                    actualStep: actualStep,
                    totalSteps: totalSteps,
                    log: Log
                );

                using (var transaction = _database.Connection.BeginTransaction()) {
                    _publisherSubscriber.ProgressiveTaskPerformStepAsync(
                        message: string.Format(Strings.RemoveDocument_ProgressiveTaskPerformStep_Database_Message, command.Document.FileName),
                        actualStep: ++actualStep,
                        totalSteps: totalSteps,
                        log: Log
                    );

                    _database.ExecuteScalar(SQL.RemoveDocument, parameters: new[] {
                        Parameter.CreateInputParameter(nameof(DocumentEntity.ID), command.Document.DocumentID, DbType.Int32)
                    });

                    if (!cancellationToken.IsCancellationRequested) {
                        transaction.Commit();

                        _publisherSubscriber.ProgressiveTaskPerformStepAsync(
                            message: string.Format(Strings.RemoveDocument_ProgressiveTaskPerformStep_Index_Message, command.Document.FileName),
                            actualStep: ++actualStep,
                            totalSteps: totalSteps,
                            log: Log
                        );

                        _indexProvider
                            .GetOrCreate(command.Document.DocumentDirectory.Code)
                            .DeleteDocuments(command.Document.DocumentID.ToString());
                    }
                }
            }, cancellationToken)
            .ContinueWith(_publisherSubscriber.TaskContinuation, new ProgressiveTaskContinuationInfo {
                ActualStep = actualStep,
                TotalSteps = totalSteps,
                Log = Log
            });
        }

        #endregion ICommandHandler<RemoveDocumentCommand> Members
    }
}