using System.Data;
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

    public sealed class RemoveDocumentDirectoryCommand : ICommand {

        #region Public Properties

        public DocumentDirectoryDto DocumentDirectory { get; set; }

        #endregion Public Properties
    }

    public sealed class RemoveDocumentDirectoryCommandHandler : ICommandHandler<RemoveDocumentDirectoryCommand> {

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

        public RemoveDocumentDirectoryCommandHandler(IDatabase database, IIndexProvider indexProvider, IPublisherSubscriber publisherSubscriber) {
            Prevent.ParameterNull(database, nameof(database));
            Prevent.ParameterNull(indexProvider, nameof(indexProvider));
            Prevent.ParameterNull(publisherSubscriber, nameof(publisherSubscriber));

            _database = database;
            _indexProvider = indexProvider;
            _publisherSubscriber = publisherSubscriber;
        }

        #endregion Public Constructors

        #region ICommandHandler<RemoveDocumentDirectoryCommand> Members

        public Task HandleAsync(RemoveDocumentDirectoryCommand command, CancellationToken cancellationToken = default(CancellationToken)) {
            var actualStep = 0;
            var totalSteps = 2;

            return Task.Run(() => {
                _publisherSubscriber.ProgressiveTaskStartAsync(
                    title: Strings.RemoveDocumentDirectory_ProgressiveTask_Title,
                    actualStep: actualStep,
                    totalSteps: totalSteps
                );

                using (var transaction = _database.Connection.BeginTransaction()) {
                    _publisherSubscriber.ProgressiveTaskPerformStepAsync(
                        message: string.Format(Strings.RemoveDocumentDirectory_ProgressiveTaskPerformStep_Database_Message, command.DocumentDirectory.Code),
                        actualStep: ++actualStep,
                        totalSteps: totalSteps
                    );

                    _database.ExecuteScalar(SQL.RemoveDocumentDirectory, parameters: new[] {
                        Parameter.CreateInputParameter(nameof(DocumentDirectoryEntity.ID), command.DocumentDirectory.DocumentDirectoryID, DbType.Int32)
                    });

                    if (!cancellationToken.IsCancellationRequested) {
                        transaction.Commit();

                        _publisherSubscriber.ProgressiveTaskPerformStepAsync(
                            message: string.Format(Strings.RemoveDocumentDirectory_ProgressiveTaskPerformStep_Index_Message, command.DocumentDirectory.Code),
                            actualStep: ++actualStep,
                            totalSteps: totalSteps
                        );

                        _indexProvider
                            .Delete(command.DocumentDirectory.Code);
                    }
                }
            }, cancellationToken)
            .ContinueWith(_publisherSubscriber.TaskContinuation, new ProgressiveTaskContinuationInfo {
                ActualStep = actualStep,
                TotalSteps = totalSteps,
                Log = Log
            });
        }

        #endregion ICommandHandler<RemoveDocumentDirectoryCommand> Members
    }
}