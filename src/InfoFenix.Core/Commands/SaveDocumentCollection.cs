using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using InfoFenix.Core.Cqrs;
using InfoFenix.Core.Data;
using InfoFenix.Core.Dto;
using InfoFenix.Core.Logging;
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

        #endregion Private Read-Only Fields

        #region Public Properties

        private ILogger _log;

        public ILogger Log {
            get { return _log ?? NullLogger.Instance; }
            set { _log = value ?? NullLogger.Instance; }
        }

        #endregion Public Properties

        #region Public Constructors

        public SaveDocumentCollectionCommandHandler(IDatabase database) {
            Prevent.ParameterNull(database, nameof(database));

            _database = database;
        }

        #endregion Public Constructors

        #region ICommandHandler<SaveDocumentCollectionCommand> Members

        public Task HandleAsync(SaveDocumentCollectionCommand command, CancellationToken cancellationToken = default(CancellationToken), IProgress<ProgressInfo> progress = null) {
            return Task.Run(() => {
                var actualStep = 0;
                var totalSteps = command.Documents.Count;

                try {
                    progress.Start(totalSteps, Resource.SaveDocumentCollection_Progress_Start_Title);
                    using (var transaction = _database.Connection.BeginTransaction()) {
                        foreach (var document in command.Documents) {
                            progress.PerformStep(++actualStep, totalSteps, Resource.SaveDocumentCollection_Progress_Step_Message, document.FileName);

                            if (cancellationToken.IsCancellationRequested) {
                                progress.Cancel(actualStep, totalSteps);

                                cancellationToken.ThrowIfCancellationRequested();
                            }

                            var result = _database.ExecuteScalar(Resource.SaveDocumentSQL, parameters: new[] {
                                Parameter.CreateInputParameter(Common.DatabaseSchema.Documents.DocumentID, document.DocumentID > 0 ? (object)document.DocumentID : DBNull.Value, DbType.Int32),
                                Parameter.CreateInputParameter(Common.DatabaseSchema.Documents.DocumentDirectoryID, command.DocumentDirectoryID, DbType.Int32),
                                Parameter.CreateInputParameter(Common.DatabaseSchema.Documents.Path, document.Path),
                                Parameter.CreateInputParameter(Common.DatabaseSchema.Documents.LastWriteTime, document.LastWriteTime, DbType.DateTime),
                                Parameter.CreateInputParameter(Common.DatabaseSchema.Documents.Code, document.Code, DbType.Int32),
                                Parameter.CreateInputParameter(Common.DatabaseSchema.Documents.Indexed, document.Indexed ? 1 : 0, DbType.Int32),
                                Parameter.CreateInputParameter(Common.DatabaseSchema.Documents.Content, document.Content ?? string.Empty),
                                Parameter.CreateInputParameter(Common.DatabaseSchema.Documents.Payload, document.Payload != null ? (object)document.Payload : DBNull.Value, DbType.Binary)
                            });
                            if (document.DocumentID <= 0) { document.DocumentID = Convert.ToInt32(result); }
                        }

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

        #endregion ICommandHandler<SaveDocumentCollectionCommand> Members
    }
}