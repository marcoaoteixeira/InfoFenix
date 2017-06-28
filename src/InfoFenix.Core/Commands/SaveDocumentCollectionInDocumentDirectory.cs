using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using InfoFenix.Core.Cqrs;
using InfoFenix.Core.Data;
using InfoFenix.Core.Dto;
using InfoFenix.Core.Logging;
using InfoFenix.Core.Office;
using Resource = InfoFenix.Core.Resources.Resources;

namespace InfoFenix.Core.Commands {

    public sealed class SaveDocumentCollectionInDocumentDirectoryCommand : ICommand {

        #region Public Properties

        public DocumentDirectoryDto DocumentDirectory { get; set; }

        #endregion Public Properties
    }

    public sealed class SaveDocumentCollectionInDocumentDirectoryCommandHandler : ICommandHandler<SaveDocumentCollectionInDocumentDirectoryCommand> {

        #region Private Read-Only Fields

        private readonly IDatabase _database;
        private readonly IWordApplication _wordApplication;

        #endregion Private Read-Only Fields

        #region Public Properties

        private ILogger _log;

        public ILogger Log {
            get { return _log ?? NullLogger.Instance; }
            set { _log = value ?? NullLogger.Instance; }
        }

        #endregion Public Properties

        #region Public Constructors

        public SaveDocumentCollectionInDocumentDirectoryCommandHandler(IDatabase database, IWordApplication wordApplication) {
            Prevent.ParameterNull(database, nameof(database));
            Prevent.ParameterNull(wordApplication, nameof(wordApplication));

            _database = database;
            _wordApplication = wordApplication;
        }

        #endregion Public Constructors

        #region Private Methods

        private byte[] ReadFileContent(string filePath) {
            byte[] result = null;
            try {
                using (var wordDocument = _wordApplication.Open(filePath)) {
                    result = Encoding.UTF8.GetBytes(wordDocument.GetText());
                }
            } catch (Exception ex) { Log.Error(ex, $"CANNOT OPEN/READ FILE: {filePath}"); }
            return result;
        }

        private void SaveDocument(int documentDiretoryID, DocumentDto document) {
            var result = _database.ExecuteScalar(Resource.SaveDocumentSQL, parameters: new[] {
                Parameter.CreateInputParameter(Common.DatabaseSchema.Documents.DocumentID, document.DocumentID > 0 ? (object)document.DocumentID : DBNull.Value, DbType.Int32),
                Parameter.CreateInputParameter(Common.DatabaseSchema.Documents.DocumentDirectoryID, documentDiretoryID, DbType.Int32),
                Parameter.CreateInputParameter(Common.DatabaseSchema.Documents.Path, document.Path),
                Parameter.CreateInputParameter(Common.DatabaseSchema.Documents.LastWriteTime, document.LastWriteTime, DbType.DateTime),
                Parameter.CreateInputParameter(Common.DatabaseSchema.Documents.Code, document.Code, DbType.Int32),
                Parameter.CreateInputParameter(Common.DatabaseSchema.Documents.Indexed, document.Indexed ? 1 : 0, DbType.Int32),
                Parameter.CreateInputParameter(Common.DatabaseSchema.Documents.Payload, document.Payload != null ? (object)document.Payload : DBNull.Value, DbType.Binary)
            });
            if (document.DocumentID <= 0) { document.DocumentID = Convert.ToInt32(result); }
        }

        #endregion Private Methods

        #region ICommandHandler<SaveDocumentCollectionInDocumentDirectoryCommand> Members

        public Task HandleAsync(SaveDocumentCollectionInDocumentDirectoryCommand command, CancellationToken cancellationToken = default(CancellationToken), IProgress<ProgressInfo> progress = null) {
            return Task.Run(() => {
                var physicalFiles = Common.GetDocFiles(command.DocumentDirectory.Path);
                var actualStep = 0;
                var totalSteps = physicalFiles.Length;

                try {
                    progress.Start(totalSteps, Resource.SaveDocumentCollectionInDocumentDirectory_Progress_Start_Title);

                    using (var transaction = _database.Connection.BeginTransaction()) {
                        foreach (var filePath in physicalFiles) {
                            if (cancellationToken.IsCancellationRequested) {
                                progress.Cancel(actualStep, totalSteps);

                                cancellationToken.ThrowIfCancellationRequested();
                            }

                            progress.PerformStep(++actualStep, totalSteps, Resource.SaveDocumentCollectionInDocumentDirectory_Progress_Step_Message, Path.GetFileName(filePath));

                            var document = command.DocumentDirectory.Documents.SingleOrDefault(_ => string.Equals(_.Path, filePath, StringComparison.CurrentCultureIgnoreCase));
                            var physicalFileLastWriteTime = File.GetLastWriteTime(filePath);

                            // If database document exists and last write time is equals to the physical file last write time
                            // Ignores and move next.
                            if (document != null && document.LastWriteTime == physicalFileLastWriteTime) { continue; }

                            // If database document exists and last write time is NOT equals to the physical file last write time
                            // Updates the database document.
                            if (document != null && document.LastWriteTime != physicalFileLastWriteTime) {
                                document.Indexed = false;
                                document.LastWriteTime = physicalFileLastWriteTime;
                                document.Payload = ReadFileContent(filePath);
                            }

                            // If database document does not exists, create it.
                            if (document == null) {
                                document = new DocumentDto {
                                    Code = Common.ExtractCodeFromFilePath(filePath),
                                    DocumentDirectory = command.DocumentDirectory,
                                    DocumentID = 0,
                                    Indexed = false,
                                    LastWriteTime = physicalFileLastWriteTime,
                                    Path = filePath,
                                    Payload = ReadFileContent(filePath)
                                };

                                command.DocumentDirectory.Documents.Add(document);
                            }

                            // Persists the document.
                            SaveDocument(command.DocumentDirectory.DocumentDirectoryID, document);
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

        #endregion ICommandHandler<SaveDocumentCollectionInDocumentDirectoryCommand> Members
    }
}