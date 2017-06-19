using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using InfoFenix.Core.Cqrs;
using InfoFenix.Core.Data;
using InfoFenix.Core.Dto;
using InfoFenix.Core.Entities;
using InfoFenix.Core.Logging;
using InfoFenix.Core.PubSub;
using SQL = InfoFenix.Core.Resources.Resources;
using Strings = InfoFenix.Core.Resources.Resources;

namespace InfoFenix.Core.Commands {

    public sealed class SaveDocumentCommand : ICommand {

        #region Public Properties

        public DocumentDto Document { get; set; }

        #endregion Public Properties
    }

    public sealed class SaveDocumentCommandHandler : ICommandHandler<SaveDocumentCommand> {

        #region Private Read-Only Fields

        private readonly IDatabase _database;
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

        public SaveDocumentCommandHandler(IDatabase database, IPublisherSubscriber publisherSubscriber) {
            Prevent.ParameterNull(database, nameof(database));
            Prevent.ParameterNull(publisherSubscriber, nameof(publisherSubscriber));

            _database = database;
            _publisherSubscriber = publisherSubscriber;
        }

        #endregion Public Constructors

        #region ICommandHandler<SaveDocumentCommand> Members

        public Task HandleAsync(SaveDocumentCommand command, CancellationToken cancellationToken = default(CancellationToken)) {
            var actualStep = 0;
            var totalSteps = 1;

            return Task.Run(() => {
                _publisherSubscriber.ProgressiveTaskStartAsync(
                    title: Strings.SaveDocument_ProgressiveTaskStart_Title,
                    actualStep: actualStep,
                    totalSteps: totalSteps
                );

                using (var transaction = _database.Connection.BeginTransaction()) {
                    _publisherSubscriber.ProgressiveTaskPerformStepAsync(
                        title: string.Format(Strings.SaveDocument_ProgressiveTaskPerformStep_Message, command.Document.FileName),
                        actualStep: ++actualStep,
                        totalSteps: totalSteps
                    );

                    var result = _database.ExecuteScalar(SQL.SaveDocument, parameters: new[] {
                        Parameter.CreateInputParameter(nameof(DocumentEntity.ID), command.Document.DocumentID > 0 ? (object)command.Document.DocumentID : DBNull.Value, DbType.Int32),
                        Parameter.CreateInputParameter(nameof(DocumentEntity.DocumentDirectoryID), command.Document.DocumentDirectory.DocumentDirectoryID, DbType.Int32),
                        Parameter.CreateInputParameter(nameof(DocumentEntity.Path), command.Document.Path),
                        Parameter.CreateInputParameter(nameof(DocumentEntity.LastWriteTime), command.Document.LastWriteTime, DbType.DateTime),
                        Parameter.CreateInputParameter(nameof(DocumentEntity.Code), command.Document.Code, DbType.Int32),
                        Parameter.CreateInputParameter(nameof(DocumentEntity.Indexed), command.Document.Indexed ? 1 : 0, DbType.Int32),
                        Parameter.CreateInputParameter(nameof(DocumentEntity.Payload), command.Document.Payload != null ? (object)command.Document.Payload : DBNull.Value, DbType.Binary)
                    });

                    if (command.Document.DocumentID <= 0) { command.Document.DocumentID = Convert.ToInt32(result); }

                    if (!cancellationToken.IsCancellationRequested) {
                        transaction.Commit();
                    }
                }
            }, cancellationToken)
            .ContinueWith(_publisherSubscriber.TaskContinuation, new ProgressiveTaskContinuationInfo {
                ActualStep = actualStep,
                TotalSteps = totalSteps,
                Log = Log
            });
        }

        #endregion ICommandHandler<SaveDocumentCommand> Members
    }
}