using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using InfoFenix.Core.Cqrs;
using InfoFenix.Core.Data;
using InfoFenix.Core.Dto;
using InfoFenix.Core.Logging;
using Resource = InfoFenix.Core.Resources.Resources;

namespace InfoFenix.Core.Commands {

    public sealed class SaveDocumentCommand : ICommand {

        #region Public Properties

        public DocumentDto Document { get; set; }

        #endregion Public Properties
    }

    public sealed class SaveDocumentCommandHandler : ICommandHandler<SaveDocumentCommand> {

        #region Private Read-Only Fields

        private readonly IDatabase _database;

        #endregion Private Read-Only Fields

        #region Public Properties

        private ILogger _log;

        public ILogger Log {
            get { return _log ?? NullLogger.Instance; }
            set { _log = value ?? NullLogger.Instance; }
        }

        #endregion Public Properties

        #region Public Constructors

        public SaveDocumentCommandHandler(IDatabase database) {
            Prevent.ParameterNull(database, nameof(database));

            _database = database;
        }

        #endregion Public Constructors

        #region ICommandHandler<SaveDocumentCommand> Members

        public Task HandleAsync(SaveDocumentCommand command, CancellationToken cancellationToken = default(CancellationToken), IProgress<ProgressInfo> progress = null) {
            return Task.Run(() => {
                var actualStep = 0;
                var totalSteps = 1;

                try {
                    progress.Start(totalSteps, Resource.SaveDocument_Progress_Start_Title);

                    using (var transaction = _database.Connection.BeginTransaction()) {
                        progress.PerformStep(++actualStep, totalSteps, Resource.SaveDocument_Progress_Step_Message, command.Document.FileName);

                        var result = _database.ExecuteScalar(Resource.SaveDocumentSQL, parameters: new[] {
                            Parameter.CreateInputParameter(Common.DatabaseSchema.Documents.DocumentID, command.Document.DocumentID > 0 ? (object)command.Document.DocumentID : DBNull.Value, DbType.Int32),
                            Parameter.CreateInputParameter(Common.DatabaseSchema.Documents.DocumentDirectoryID, command.Document.DocumentDirectory.DocumentDirectoryID, DbType.Int32),
                            Parameter.CreateInputParameter(Common.DatabaseSchema.Documents.Path, command.Document.Path),
                            Parameter.CreateInputParameter(Common.DatabaseSchema.Documents.LastWriteTime, command.Document.LastWriteTime, DbType.DateTime),
                            Parameter.CreateInputParameter(Common.DatabaseSchema.Documents.Code, command.Document.Code, DbType.Int32),
                            Parameter.CreateInputParameter(Common.DatabaseSchema.Documents.Indexed, command.Document.Indexed ? 1 : 0, DbType.Int32),
                            Parameter.CreateInputParameter(Common.DatabaseSchema.Documents.Payload, command.Document.Payload != null ? (object)command.Document.Payload : DBNull.Value, DbType.Binary)
                        });
                        if (command.Document.DocumentID <= 0) { command.Document.DocumentID = Convert.ToInt32(result); }

                        if (cancellationToken.IsCancellationRequested) {
                            progress.Cancel(actualStep, totalSteps);

                            cancellationToken.ThrowIfCancellationRequested();
                        }
                        transaction.Commit();
                    }

                    progress.Complete(actualStep, totalSteps);
                } catch (Exception ex) { progress.Error(actualStep, totalSteps, ex.Message); throw; }
            }, cancellationToken);
        }

        #endregion ICommandHandler<SaveDocumentCommand> Members
    }
}