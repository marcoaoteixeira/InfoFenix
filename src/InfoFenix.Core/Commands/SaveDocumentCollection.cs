using System;
using System.Collections.Generic;
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

        public Task HandleAsync(SaveDocumentCollectionCommand command, IProgress<ProgressArguments> progress = null, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => {
                var actualStep = 0;
                var totalSteps = command.Documents.Count;

                _publisherSubscriber.Publish(new ProgressiveTaskStartNotification {
                    Arguments = new ProgressiveTaskArguments {
                        Title = Resource.SaveDocumentCollection_ProgressiveTaskStart_Title,
                        ActualStep = actualStep,
                        TotalSteps = totalSteps
                    }
                });
                
                using (var transaction = _database.Connection.BeginTransaction()) {
                    try {
                        foreach (var document in command.Documents) {
                            if (cancellationToken.IsCancellationRequested) {
                                _publisherSubscriber.Publish(new ProgressiveTaskCancelNotification {
                                    Arguments = new ProgressiveTaskArguments {
                                        ActualStep = actualStep,
                                        TotalSteps = totalSteps
                                    }
                                });

                                cancellationToken.ThrowIfCancellationRequested();
                            }

                            _publisherSubscriber.Publish(new ProgressiveTaskPerformStepNotification {
                                Arguments = new ProgressiveTaskArguments {
                                    Message = string.Format(Resource.SaveDocumentCollection_ProgressiveTaskPerformStep_Message, document.FileName),
                                    ActualStep = ++actualStep,
                                    TotalSteps = totalSteps
                                }
                            });

                            var result = _database.ExecuteScalar(Resource.SaveDocumentSQL, parameters: new[] {
                                Parameter.CreateInputParameter(Common.DatabaseSchema.Documents.DocumentID, document.DocumentID > 0 ? (object)document.DocumentID : DBNull.Value, DbType.Int32),
                                Parameter.CreateInputParameter(Common.DatabaseSchema.Documents.DocumentDirectoryID, command.DocumentDirectoryID, DbType.Int32),
                                Parameter.CreateInputParameter(Common.DatabaseSchema.Documents.Path, document.Path),
                                Parameter.CreateInputParameter(Common.DatabaseSchema.Documents.LastWriteTime, document.LastWriteTime, DbType.DateTime),
                                Parameter.CreateInputParameter(Common.DatabaseSchema.Documents.Code, document.Code, DbType.Int32),
                                Parameter.CreateInputParameter(Common.DatabaseSchema.Documents.Indexed, document.Indexed ? 1 : 0, DbType.Int32),
                                Parameter.CreateInputParameter(Common.DatabaseSchema.Documents.Payload, document.Payload != null ? (object)document.Payload : DBNull.Value, DbType.Binary)
                            });
                            if (document.DocumentID <= 0) { document.DocumentID = Convert.ToInt32(result); }
                        }

                        if (cancellationToken.IsCancellationRequested) {
                            _publisherSubscriber.Publish(new ProgressiveTaskCancelNotification {
                                Arguments = new ProgressiveTaskArguments {
                                    ActualStep = actualStep,
                                    TotalSteps = totalSteps
                                }
                            });

                            cancellationToken.ThrowIfCancellationRequested();
                        }

                        transaction.Commit();
                    } catch (Exception ex) {
                        _publisherSubscriber.Publish(new ProgressiveTaskErrorNotification {
                            Arguments = new ProgressiveTaskArguments {
                                Error = ex.Message,
                                ActualStep = actualStep,
                                TotalSteps = totalSteps
                            }
                        });

                        throw;
                    }
                }
            }, cancellationToken);
        }

        #endregion ICommandHandler<SaveDocumentCollectionCommand> Members
    }
}