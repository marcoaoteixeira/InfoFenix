using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using InfoFenix.Core.Cqrs;
using InfoFenix.Core.Data;
using InfoFenix.Core.Dto;
using InfoFenix.Core.Logging;
using InfoFenix.Core.PubSub;
using Resource = InfoFenix.Core.Resources.Resources;

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

        public Task HandleAsync(SaveDocumentCommand command, IProgress<ProgressArguments> progress = null, CancellationToken cancellationToken = default(CancellationToken)) {
            var info = new ProgressiveTaskContinuationInfo {
                Log = Log,
                TotalSteps = 1
            };

            return Task.Run(() => {
                _publisherSubscriber.ProgressiveTaskStart(
                    title: Resource.SaveDocument_ProgressiveTaskStart_Title,
                    actualStep: info.ActualStep,
                    totalSteps: info.TotalSteps
                );

                using (var transaction = _database.Connection.BeginTransaction()) {
                    _publisherSubscriber.ProgressiveTaskPerformStep(
                        title: string.Format(Resource.SaveDocument_ProgressiveTaskPerformStep_Message, command.Document.FileName),
                        actualStep: ++info.ActualStep,
                        totalSteps: info.TotalSteps
                    );

                    var result = _database.ExecuteScalar(Resource.SaveDocumentSQL, parameters: new[] {
                        Parameter.CreateInputParameter(Common.DatabaseSchema.Documents.DocumentID, command.Document.DocumentID > 0 ? (object)command.Document.DocumentID : DBNull.Value, DbType.Int32),
                        Parameter.CreateInputParameter(Common.DatabaseSchema.Documents.DocumentDirectoryID, command.Document.DocumentDirectory.DocumentDirectoryID, DbType.Int32),
                        Parameter.CreateInputParameter(Common.DatabaseSchema.Documents.Path, command.Document.Path),
                        Parameter.CreateInputParameter(Common.DatabaseSchema.Documents.LastWriteTime, command.Document.LastWriteTime, DbType.DateTime),
                        Parameter.CreateInputParameter(Common.DatabaseSchema.Documents.Code, command.Document.Code, DbType.Int32),
                        Parameter.CreateInputParameter(Common.DatabaseSchema.Documents.Indexed, command.Document.Indexed ? 1 : 0, DbType.Int32),
                        Parameter.CreateInputParameter(Common.DatabaseSchema.Documents.Payload, command.Document.Payload != null ? (object)command.Document.Payload : DBNull.Value, DbType.Binary)
                    });
                    if (command.Document.DocumentID <= 0) { command.Document.DocumentID = Convert.ToInt32(result); }

                    cancellationToken.ThrowIfCancellationRequested();
                    transaction.Commit();
                }
            }, cancellationToken)
            .ContinueWith(_publisherSubscriber.TaskContinuation, info);
        }

        #endregion ICommandHandler<SaveDocumentCommand> Members
    }
}