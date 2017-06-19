﻿using System.Collections.Generic;
using System.Data;
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

    public sealed class RemoveDocumentCollectionCommand : ICommand {

        #region Public Properties

        public string DocumentDirectoryCode { get; set; }

        public IList<DocumentDto> Documents { get; set; } = new List<DocumentDto>();

        #endregion Public Properties
    }

    public sealed class RemoveDocumentCollectionCommandHandler : ICommandHandler<RemoveDocumentCollectionCommand> {

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

        public RemoveDocumentCollectionCommandHandler(IDatabase database, IIndexProvider indexProvider, IPublisherSubscriber publisherSubscriber) {
            Prevent.ParameterNull(database, nameof(database));
            Prevent.ParameterNull(indexProvider, nameof(indexProvider));
            Prevent.ParameterNull(publisherSubscriber, nameof(publisherSubscriber));

            _database = database;
            _indexProvider = indexProvider;
            _publisherSubscriber = publisherSubscriber;
        }

        #endregion Public Constructors

        #region ICommandHandler<RemoveDocumentCollectionCommand> Members

        public Task HandleAsync(RemoveDocumentCollectionCommand command, CancellationToken cancellationToken = default(CancellationToken)) {
            var documentIDs = command.Documents.Select(_ => _.DocumentID).ToArray();
            var actualStep = 0;
            var totalSteps = documentIDs.Length + 1;

            return Task.Run(() => {
                _publisherSubscriber.ProgressiveTaskStartAsync(
                    title: Strings.RemoveDocumentCollection_ProgressiveTaskStart_Title,
                    totalSteps: totalSteps
                );

                using (var transaction = _database.Connection.BeginTransaction()) {
                    foreach (var document in command.Documents) {
                        _publisherSubscriber.ProgressiveTaskPerformStepAsync(
                            message: string.Format(Strings.RemoveDocumentCollection_ProgressiveTaskPerformStep_Database_Message, document.Code),
                            actualStep: ++actualStep,
                            totalSteps: totalSteps
                        );

                        _database.ExecuteScalar(SQL.RemoveDocument, parameters: new[] {
                            Parameter.CreateInputParameter(nameof(DocumentEntity.ID), document.DocumentID, DbType.Int32)
                        });

                        if (cancellationToken.IsCancellationRequested) { break; }
                    }

                    if (!cancellationToken.IsCancellationRequested) {
                        transaction.Commit();

                        _publisherSubscriber.ProgressiveTaskPerformStepAsync(
                            message: Strings.RemoveDocumentCollection_ProgressiveTaskPerformStep_Index_Message,
                            actualStep: ++actualStep,
                            totalSteps: totalSteps
                        );

                        _indexProvider
                            .GetOrCreate(command.DocumentDirectoryCode)
                            .DeleteDocuments(command.Documents.Select(_ => _.DocumentID.ToString()).ToArray());
                    }
                }
            }, cancellationToken)
            .ContinueWith(_publisherSubscriber.TaskContinuation, new ProgressiveTaskContinuationInfo {
                ActualStep = actualStep,
                TotalSteps = totalSteps,
                Log = Log
            });
        }

        #endregion ICommandHandler<RemoveDocumentCollectionCommand> Members
    }
}