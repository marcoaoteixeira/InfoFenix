using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
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

        public Task HandleAsync(CleanDocumentDirectoryCommand command, CancellationToken cancellationToken = default(CancellationToken)) {
            var actualStep = 0;
            var totalSteps = 0;

            return Task.Run(() => {
                var filesOnDatabase = command.DocumentDirectory.Documents.Select(_ => _.Path).ToArray();
                var filesOnDisk = Common.GetDocFiles(command.DocumentDirectory.Path);
                var toRemove = filesOnDatabase.Except(filesOnDisk).ToArray();
                var documentsToRemove = new List<DocumentDto>();

                totalSteps = toRemove.Length + 1;

                _publisherSubscriber.ProgressiveTaskStartAsync(
                    title: Strings.CleanDocumentDirectory_ProgressiveTaskStart_Title,
                    actualStep: actualStep,
                    totalSteps: totalSteps,
                    log: Log
                );

                using (var transaction = _database.Connection.BeginTransaction()) {
                    foreach (var filePath in toRemove) {
                        _publisherSubscriber.ProgressiveTaskPerformStepAsync(
                            message: string.Format(Strings.CleanDocumentDirectory_ProgressiveTaskPerformStep_Database_Message, Path.GetFileName(filePath)),
                            actualStep: ++actualStep,
                            totalSteps: totalSteps,
                            log: Log
                        );

                        var document = command.DocumentDirectory.Documents.SingleOrDefault(_ => _.Path == filePath);
                        _database.ExecuteNonQuery(SQL.RemoveDocument, parameters: new[] {
                            Parameter.CreateInputParameter(nameof(DocumentEntity.ID), document.DocumentID, DbType.Int32)
                        });
                        documentsToRemove.Add(document);

                        if (cancellationToken.IsCancellationRequested) { break; }
                    }

                    if (!cancellationToken.IsCancellationRequested) {
                        transaction.Commit();

                        _publisherSubscriber.ProgressiveTaskPerformStepAsync(
                            message: string.Format(Strings.CleanDocumentDirectory_ProgressiveTaskPerformStep_Index_Message, command.DocumentDirectory.Label),
                            actualStep: ++actualStep,
                            totalSteps: totalSteps,
                            log: Log
                        );

                        _indexProvider
                            .GetOrCreate(command.DocumentDirectory.Code)
                            .DeleteDocuments(documentsToRemove.Select(_ => _.DocumentID.ToString()).ToArray());
                    }
                }
            }, cancellationToken)
            .ContinueWith(_publisherSubscriber.TaskContinuation, new ProgressiveTaskContinuationInfo {
                ActualStep = actualStep,
                TotalSteps = totalSteps,
                Log = Log
            });
        }

        #endregion ICommandHandler<CleanDocumentDirectoryCommand> Members
    }
}