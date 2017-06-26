using System;
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

        public Task HandleAsync(RemoveDocumentDirectoryCommand command, IProgress<ProgressArguments> progress = null, CancellationToken cancellationToken = default(CancellationToken)) {
            var info = new ProgressiveTaskContinuationInfo {
                Log = Log,
                TotalSteps = 2
            };

            return Task.Run(() => {
                _publisherSubscriber.ProgressiveTaskStart(
                    title: Resource.RemoveDocumentDirectory_ProgressiveTask_Title,
                    actualStep: info.ActualStep,
                    totalSteps: info.TotalSteps
                );

                using (var transaction = _database.Connection.BeginTransaction()) {
                    _publisherSubscriber.ProgressiveTaskPerformStep(
                        message: string.Format(Resource.RemoveDocumentDirectory_ProgressiveTaskPerformStep_Database_Message, command.DocumentDirectory.Code),
                        actualStep: ++info.ActualStep,
                        totalSteps: info.TotalSteps
                    );

                    _database.ExecuteScalar(Resource.RemoveDocumentDirectorySQL, parameters: new[] {
                        Parameter.CreateInputParameter(Common.DatabaseSchema.DocumentDirectories.DocumentDirectoryID, command.DocumentDirectory.DocumentDirectoryID, DbType.Int32)
                    });

                    cancellationToken.ThrowIfCancellationRequested();
                    transaction.Commit();

                    _publisherSubscriber.ProgressiveTaskPerformStep(
                        message: string.Format(Resource.RemoveDocumentDirectory_ProgressiveTaskPerformStep_Index_Message, command.DocumentDirectory.Code),
                        actualStep: ++info.ActualStep,
                        totalSteps: info.TotalSteps
                    );

                    _indexProvider
                        .Delete(command.DocumentDirectory.Code);
                }
            }, cancellationToken)
            .ContinueWith(_publisherSubscriber.TaskContinuation, info);
        }

        #endregion ICommandHandler<RemoveDocumentDirectoryCommand> Members
    }
}