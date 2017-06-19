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

    public sealed class SaveDocumentDirectoryCommand : ICommand {

        #region Public Properties

        public DocumentDirectoryDto DocumentDirectory { get; set; }

        #endregion Public Properties
    }

    public sealed class SaveDocumentDirectoryCommandHandler : ICommandHandler<SaveDocumentDirectoryCommand> {

        #region Private Read-Only Fields

        private readonly IDatabase _database;
        private readonly IPublisherSubscriber _publisherSuscriber;

        #endregion Private Read-Only Fields

        #region Public Properties

        private ILogger _log;

        public ILogger Log {
            get { return _log ?? NullLogger.Instance; }
            set { _log = value ?? NullLogger.Instance; }
        }

        #endregion Public Properties

        #region Public Constructors

        public SaveDocumentDirectoryCommandHandler(IDatabase database, IPublisherSubscriber publisherSuscriber) {
            Prevent.ParameterNull(database, nameof(database));
            Prevent.ParameterNull(publisherSuscriber, nameof(publisherSuscriber));

            _database = database;
            _publisherSuscriber = publisherSuscriber;
        }

        #endregion Public Constructors

        #region ICommandHandler<SaveDocumentDirectoryCommand> Members

        public Task HandleAsync(SaveDocumentDirectoryCommand command, CancellationToken cancellationToken = default(CancellationToken)) {
            var actualStep = 0;
            var totalSteps = 1;

            return Task.Run(() => {
                _publisherSuscriber.ProgressiveTaskStartAsync(
                    title: Strings.SaveDocumentDirectory_ProgressiveTaskStart_Title,
                    actualStep: actualStep,
                    totalSteps: totalSteps
                );

                using (var transaction = _database.Connection.BeginTransaction()) {
                    _publisherSuscriber.ProgressiveTaskStartAsync(
                        title: string.Format(Strings.SaveDocumentDirectory_ProgressiveTaskPerformStep_Message, command.DocumentDirectory.Label),
                        actualStep: ++actualStep,
                        totalSteps: totalSteps
                    );

                    var result = _database.ExecuteScalar(SQL.SaveDocumentDirectory, parameters: new[] {
                        Parameter.CreateInputParameter(nameof(DocumentDirectoryEntity.ID), command.DocumentDirectory.DocumentDirectoryID != 0 ? (object)command.DocumentDirectory.DocumentDirectoryID : DBNull.Value, DbType.Int32),
                        Parameter.CreateInputParameter(nameof(DocumentDirectoryEntity.Label), command.DocumentDirectory.Label),
                        Parameter.CreateInputParameter(nameof(DocumentDirectoryEntity.Path), command.DocumentDirectory.Path),
                        Parameter.CreateInputParameter(nameof(DocumentDirectoryEntity.Code), command.DocumentDirectory.Code),
                        Parameter.CreateInputParameter(nameof(DocumentDirectoryEntity.Watch), command.DocumentDirectory.Watch ? 1 : 0, DbType.Int32),
                        Parameter.CreateInputParameter(nameof(DocumentDirectoryEntity.Index), command.DocumentDirectory.Index ? 1 : 0, DbType.Int32)
                    });
                    if (command.DocumentDirectory.DocumentDirectoryID <= 0) { command.DocumentDirectory.DocumentDirectoryID = Convert.ToInt32(result); }

                    if (!cancellationToken.IsCancellationRequested) { transaction.Commit(); }
                }
            }, cancellationToken)
            .ContinueWith(_publisherSuscriber.TaskContinuation, new ProgressiveTaskContinuationInfo {
                ActualStep = actualStep,
                TotalSteps = totalSteps,
                Log = Log
            });
        }

        #endregion ICommandHandler<SaveDocumentDirectoryCommand> Members
    }
}