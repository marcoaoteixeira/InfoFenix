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

        #endregion Private Read-Only Fields

        #region Public Properties

        private ILogger _log;

        public ILogger Log {
            get { return _log ?? NullLogger.Instance; }
            set { _log = value ?? NullLogger.Instance; }
        }

        #endregion Public Properties

        #region Public Constructors

        public CleanDocumentDirectoryCommandHandler(IDatabase database, IIndexProvider indexProvider) {
            Prevent.ParameterNull(database, nameof(database));
            Prevent.ParameterNull(indexProvider, nameof(indexProvider));

            _database = database;
            _indexProvider = indexProvider;
        }

        #endregion Public Constructors

        #region ICommandHandler<CleanDocumentDirectoryCommand> Members

        public Task HandleAsync(CleanDocumentDirectoryCommand command, CancellationToken cancellationToken = default(CancellationToken), IProgress<ProgressInfo> progress = null) {
            return Task.Run(() => {
                var actualStep = 0;
                var totalSteps = 0;
                try {
                    var filesOnDatabase = command.DocumentDirectory.Documents.Select(_ => _.Path).ToArray();
                    var filesOnDisk = Common.GetDocFiles(command.DocumentDirectory.Path);
                    var removeFromDatabase = filesOnDatabase.Except(filesOnDisk).ToArray();
                    var removeFromIndex = new List<DocumentDto>();

                    totalSteps = removeFromDatabase.Length + 1;

                    progress.Start(totalSteps, Resource.CleanDocumentDirectory_Progress_Start_Title);

                    using (var transaction = _database.Connection.BeginTransaction()) {
                        foreach (var filePath in removeFromDatabase) {
                            progress.PerformStep(++actualStep, totalSteps, Resource.CleanDocumentDirectory_Progress_Step_Database_Message, Path.GetFileName(filePath));

                            if (cancellationToken.IsCancellationRequested) {
                                progress.Cancel(actualStep, totalSteps);

                                cancellationToken.ThrowIfCancellationRequested();
                            }

                            var document = command.DocumentDirectory.Documents.SingleOrDefault(_ => _.Path == filePath);
                            _database.ExecuteNonQuery(Resource.RemoveDocumentSQL, parameters: new[] {
                            Parameter.CreateInputParameter(Common.DatabaseSchema.Documents.DocumentID, document.DocumentID, DbType.Int32)
                        });

                            removeFromIndex.Add(document);
                        }

                        if (cancellationToken.IsCancellationRequested) {
                            progress.Cancel(actualStep, totalSteps);

                            cancellationToken.ThrowIfCancellationRequested();
                        }
                        transaction.Commit();

                        progress.PerformStep(++actualStep, totalSteps, Resource.CleanDocumentDirectory_Progress_Step_Index_Message, command.DocumentDirectory.Label);
                        _indexProvider
                            .GetOrCreate(command.DocumentDirectory.Code)
                            .DeleteDocuments(removeFromIndex.Select(_ => _.DocumentID.ToString()).ToArray());
                    }

                    progress.Complete(actualStep, totalSteps);
                } catch (Exception ex) { progress.Error(actualStep, totalSteps, ex.Message); throw; }
            }, cancellationToken);
        }

        #endregion ICommandHandler<CleanDocumentDirectoryCommand> Members
    }
}