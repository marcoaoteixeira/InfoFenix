using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
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

    public sealed class CleanDocumentDirectoryCommand : ICommand {

        #region Public Properties

        public DocumentDirectoryDto DocumentDirectory { get; set; }

        #endregion Public Properties
    }

    public sealed class CleanDocumentDirectoryCommandHandler : ICommandHandler<CleanDocumentDirectoryCommand> {

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

        public CleanDocumentDirectoryCommandHandler(IDatabase database, IIndexProvider indexProvider, IPublisherSubscriber publisherSubscriber) {
            Prevent.ParameterNull(database, nameof(database));
            Prevent.ParameterNull(indexProvider, nameof(indexProvider));
            Prevent.ParameterNull(publisherSubscriber, nameof(publisherSubscriber));
            
            _database = database;
            _indexProvider = indexProvider;
            _publisherSubscriber = publisherSubscriber;
        }

        #endregion Public Constructors

        #region ICommandHandler<CleanDocumentDirectoryCommand> Members

        public Task HandleAsync(CleanDocumentDirectoryCommand command, IProgress<ProgressArguments> progress = null, CancellationToken cancellationToken = default(CancellationToken)) {
            var info = new ProgressiveTaskContinuationInfo { Log = Log };

            return Task.Run(() => {
                var filesOnDatabase = command.DocumentDirectory.Documents.Select(_ => _.Path).ToArray();
                var filesOnDisk = Common.GetDocFiles(command.DocumentDirectory.Path);
                var removeFromDatabase = filesOnDatabase.Except(filesOnDisk).ToArray();
                var removeFromIndex = new List<DocumentDto>();

                info.TotalSteps = removeFromDatabase.Length + 1;

                _publisherSubscriber.ProgressiveTaskStart(
                    title: Resource.CleanDocumentDirectory_ProgressiveTaskStart_Title,
                    actualStep: info.ActualStep,
                    totalSteps: info.TotalSteps,
                    log: Log
                );

                using (var transaction = _database.Connection.BeginTransaction()) {
                    foreach (var filePath in removeFromDatabase) {
                        cancellationToken.ThrowIfCancellationRequested();

                        _publisherSubscriber.ProgressiveTaskPerformStep(
                            message: string.Format(Resource.CleanDocumentDirectory_ProgressiveTaskPerformStep_Database_Message, Path.GetFileName(filePath)),
                            actualStep: ++info.ActualStep,
                            totalSteps: info.TotalSteps,
                            log: Log
                        );

                        var document = command.DocumentDirectory.Documents.SingleOrDefault(_ => _.Path == filePath);
                        _database.ExecuteNonQuery(Resource.RemoveDocumentSQL, parameters: new[] {
                            Parameter.CreateInputParameter(Common.DatabaseSchema.Documents.DocumentID, document.DocumentID, DbType.Int32)
                        });

                        removeFromIndex.Add(document);
                    }

                    cancellationToken.ThrowIfCancellationRequested();
                    transaction.Commit();

                    _publisherSubscriber.ProgressiveTaskPerformStep(
                        message: string.Format(Resource.CleanDocumentDirectory_ProgressiveTaskPerformStep_Index_Message, command.DocumentDirectory.Label),
                        actualStep: ++info.ActualStep,
                        totalSteps: info.TotalSteps,
                        log: Log
                    );

                    _indexProvider
                        .GetOrCreate(command.DocumentDirectory.Code)
                        .DeleteDocuments(removeFromIndex.Select(_ => _.DocumentID.ToString()).ToArray());
                }
            }, cancellationToken)
            .ContinueWith(_publisherSubscriber.TaskContinuation, info);
        }

        #endregion ICommandHandler<CleanDocumentDirectoryCommand> Members
    }
}