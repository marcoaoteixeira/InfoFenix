using System;
using System.Collections.Generic;
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

    public sealed class SaveDocumentCollectionCommand : ICommand {

        #region Public Properties

        public int DocumentDirectoryID { get; set; }

        public IList<DocumentDto> Documents { get; set; } = new List<DocumentDto>();

        #endregion Public Properties
    }

    public sealed class SaveDocumentCollectionCommandHandler : ICommandHandler<SaveDocumentCollectionCommand> {

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

        public SaveDocumentCollectionCommandHandler(IDatabase database, IPublisherSubscriber publisherSubscriber) {
            Prevent.ParameterNull(database, nameof(database));
            Prevent.ParameterNull(publisherSubscriber, nameof(publisherSubscriber));

            _database = database;
            _publisherSubscriber = publisherSubscriber;
        }

        #endregion Public Constructors

        #region ICommandHandler<SaveDocumentCollectionCommand> Members

        public Task HandleAsync(SaveDocumentCollectionCommand command, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => {
                var totalSteps = command.Documents.Count;
                _publisherSubscriber.PublishAsync(new ProgressiveTaskStartNotification {
                    Title = Strings.SaveDocumentCollection_ProgressiveTaskStart_Title,
                    TotalSteps = totalSteps
                });

                var step = 0;
                var error = string.Empty;
                try {
                    using (var transaction = _database.Connection.BeginTransaction()) {
                        foreach (var document in command.Documents) {
                            if (cancellationToken.IsCancellationRequested) {
                                _publisherSubscriber.PublishAsync(new ProgressiveTaskCancelNotification {
                                    ActualStep = step,
                                    TotalSteps = totalSteps
                                });

                                break;
                            }

                            _publisherSubscriber.PublishAsync(new ProgressiveTaskPerformStepNotification {
                                ActualStep = ++step,
                                Message = string.Format(Strings.SaveDocumentCollection_ProgressiveTaskPerformStep_Message, document.Code),
                                TotalSteps = totalSteps
                            });

                            var result = _database.ExecuteScalar(SQL.SaveDocument, parameters: new[] {
                                Parameter.CreateInputParameter(nameof(DocumentEntity.ID), document.DocumentID > 0 ? (object)document.DocumentID : DBNull.Value, DbType.Int32),
                                Parameter.CreateInputParameter(nameof(DocumentEntity.DocumentDirectoryID), command.DocumentDirectoryID, DbType.Int32),
                                Parameter.CreateInputParameter(nameof(DocumentEntity.Path), document.Path),
                                Parameter.CreateInputParameter(nameof(DocumentEntity.LastWriteTime), document.LastWriteTime, DbType.DateTime),
                                Parameter.CreateInputParameter(nameof(DocumentEntity.Code), document.Code, DbType.Int32),
                                Parameter.CreateInputParameter(nameof(DocumentEntity.Indexed), document.Indexed ? 1 : 0, DbType.Int32),
                                Parameter.CreateInputParameter(nameof(DocumentEntity.Payload), document.Payload != null ? (object)document.Payload : DBNull.Value, DbType.Binary)
                            });
                            if (document.DocumentID <= 0) { document.DocumentID = Convert.ToInt32(result); }
                        }

                        if (!cancellationToken.IsCancellationRequested) { transaction.Commit(); }
                    }
                } catch (Exception ex) {
                    error = ex.Message;

                    _publisherSubscriber.PublishAsync(new ProgressiveTaskErrorNotification {
                        ActualStep = step,
                        Error = error,
                        TotalSteps = totalSteps
                    });

                    Log.Error(ex, error);

                    throw;
                } finally {
                    _publisherSubscriber.PublishAsync(new ProgressiveTaskCompleteNotification {
                        ActualStep = step,
                        Error = error,
                        TotalSteps = totalSteps
                    });
                }
            }, cancellationToken);
        }

        #endregion ICommandHandler<SaveDocumentCollectionCommand> Members
    }
}