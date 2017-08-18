using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using InfoFenix;
using InfoFenix.CQRS;
using InfoFenix.Data;
using InfoFenix.Dto;
using InfoFenix.Entities;
using InfoFenix.Logging;
using InfoFenix.Search;
using InfoFenix.Resources;

namespace InfoFenix.Domains.Commands {

    public sealed class RemoveDocumentDirectoryCommand : ICommand {

        #region Public Properties

        public int DocumentDirectoryID { get; set; }

        #endregion Public Properties
    }

    public sealed class RemoveDocumentDirectoryCommandHandler : ICommandHandler<RemoveDocumentDirectoryCommand> {

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

        public RemoveDocumentDirectoryCommandHandler(IDatabase database, IIndexProvider indexProvider) {
            Prevent.ParameterNull(database, nameof(database));
            Prevent.ParameterNull(indexProvider, nameof(indexProvider));

            _database = database;
            _indexProvider = indexProvider;
        }

        #endregion Public Constructors

        #region ICommandHandler<RemoveDocumentDirectoryCommand> Members

        public Task HandleAsync(RemoveDocumentDirectoryCommand command, CancellationToken cancellationToken = default(CancellationToken), IProgress<ProgressInfo> progress = null) {
            return Task.Run(() => {
                var actualStep = 0;
                var totalSteps = 2;

                try {
                    progress.Start(totalSteps, Strings.RemoveDocumentDirectory_Progress_Start_Title);

                    var documentDirectory = _database.ExecuteReaderSingle(SQLs.GetDocumentDirectory, DocumentDirectory.Map, parameters: new[] {
                        Parameter.CreateInputParameter(Common.DatabaseSchema.DocumentDirectories.DocumentDirectoryID, command.DocumentDirectoryID, DbType.Int32)
                    });

                    using (var transaction = _database.Connection.BeginTransaction()) {
                        progress.PerformStep(++actualStep, totalSteps, Strings.RemoveDocumentDirectory_Progress_Step_Database_Message, documentDirectory.Label);

                        _database.ExecuteScalar(SQLs.RemoveDocumentDirectory, parameters: new[] {
                            Parameter.CreateInputParameter(Common.DatabaseSchema.DocumentDirectories.DocumentDirectoryID, documentDirectory.DocumentDirectoryID, DbType.Int32)
                        });

                        if (cancellationToken.IsCancellationRequested) {
                            progress.Cancel(actualStep, totalSteps);

                            cancellationToken.ThrowIfCancellationRequested();
                        }
                        transaction.Commit();
                    }

                    progress.PerformStep(++actualStep, totalSteps, Strings.RemoveDocumentDirectory_Progress_Step_Index_Message, documentDirectory.Label);
                    _indexProvider
                        .Delete(documentDirectory.Code);

                    progress.Complete(actualStep, totalSteps);
                } catch (Exception ex) { progress.Error(actualStep, totalSteps, ex.Message); throw; }
            }, cancellationToken);
        }

        #endregion ICommandHandler<RemoveDocumentDirectoryCommand> Members
    }
}