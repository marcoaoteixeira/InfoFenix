using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InfoFenix.Core.CQRS;
using InfoFenix.Core.Data;
using InfoFenix.Core.Entities;
using InfoFenix.Core.Logging;
using InfoFenix.Core.Office;
using Resource = InfoFenix.Core.Resources.Resources;

namespace InfoFenix.Core.Commands {

    public sealed class SaveDocumentDirectoryDocumentsCommand : ICommand {

        #region Public Properties

        public int DocumentDirectoryID { get; set; }

        #endregion Public Properties
    }

    public sealed class SaveDocumentDirectoryDocumentsCommandHandler : ICommandHandler<SaveDocumentDirectoryDocumentsCommand> {

        #region Private Static Read-Only Fields

        private static readonly string TempFilePath = Path.Combine(Path.GetTempPath(), Path.GetTempFileName());

        #endregion Private Static Read-Only Fields

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

        public SaveDocumentDirectoryDocumentsCommandHandler(IDatabase database, IWordApplication wordApplication) {
            Prevent.ParameterNull(database, nameof(database));
            Prevent.ParameterNull(wordApplication, nameof(wordApplication));

            _database = database;
            _wordApplication = wordApplication;
        }

        #endregion Public Constructors

        #region Private Methods

        private byte[] ReadFileAsRtf(string filePath) {
            byte[] result = null;
            try {
                using (var wordDocument = _wordApplication.Open(filePath)) {
                    wordDocument.SaveAs(TempFilePath);
                }
                result = File.ReadAllBytes(TempFilePath);
            } catch { Log.Error($"CANNOT SAVE FILE: {filePath}"); }
            return result;
        }

        private string ReadFileContent(string filePath) {
            string result = null;
            try {
                using (var wordDocument = _wordApplication.Open(filePath)) {
                    result = wordDocument.GetText();
                }
            } catch { Log.Error($"CANNOT OPEN/READ FILE: {filePath}"); }
            return result;
        }

        private void SaveDocument(int documentDiretoryID, Document document) {
            var result = _database.ExecuteScalar(Resource.SaveDocumentSQL, parameters: new[] {
                Parameter.CreateInputParameter(Common.DatabaseSchema.Documents.DocumentID, document.DocumentID > 0 ? (object)document.DocumentID : DBNull.Value, DbType.Int32),
                Parameter.CreateInputParameter(Common.DatabaseSchema.Documents.DocumentDirectoryID, documentDiretoryID, DbType.Int32),
                Parameter.CreateInputParameter(Common.DatabaseSchema.Documents.Code, document.Code, DbType.Int32),
                Parameter.CreateInputParameter(Common.DatabaseSchema.Documents.Content, document.Content ?? string.Empty),
                Parameter.CreateInputParameter(Common.DatabaseSchema.Documents.Payload, document.Payload != null ? (object)document.Payload : DBNull.Value, DbType.Binary),
                Parameter.CreateInputParameter(Common.DatabaseSchema.Documents.Path, document.Path),
                Parameter.CreateInputParameter(Common.DatabaseSchema.Documents.LastWriteTime, document.LastWriteTime, DbType.DateTime),
                Parameter.CreateInputParameter(Common.DatabaseSchema.Documents.Index, document.Index, DbType.Int32)
            });
            if (document.DocumentID <= 0) { document.DocumentID = Convert.ToInt32(result); }
        }

        #endregion Private Methods

        #region ICommandHandler<SaveDocumentDirectoryDocumentsCommand> Members

        public Task HandleAsync(SaveDocumentDirectoryDocumentsCommand command, CancellationToken cancellationToken = default(CancellationToken), IProgress<ProgressInfo> progress = null) {
            return Task.Run(() => {
                var documentDirectory = _database.ExecuteReaderSingle(Resource.GetDocumentDirectorySQL, DocumentDirectory.Map, parameters: new[] {
                    Parameter.CreateInputParameter(Common.DatabaseSchema.DocumentDirectories.DocumentDirectoryID, command.DocumentDirectoryID, DbType.Int32)
                });

                var documents = _database.ExecuteReader(Resource.ListDocumentsByDocumentDirectoryNoContentSQL, Document.Map, parameters: new[] {
                    Parameter.CreateInputParameter(Common.DatabaseSchema.DocumentDirectories.DocumentDirectoryID, command.DocumentDirectoryID, DbType.Int32)
                });

                var physicalFiles = Common.GetDocumentFiles(documentDirectory.Path);
                var actualStep = 0;
                var totalSteps = physicalFiles.Length;

                try {
                    progress.Start(totalSteps, Resource.SaveDocumentDirectoryDocuments_Progress_Start_Title);

                    using (var transaction = _database.Connection.BeginTransaction()) {
                        foreach (var filePath in physicalFiles) {
                            if (cancellationToken.IsCancellationRequested) {
                                progress.Cancel(actualStep, totalSteps);

                                cancellationToken.ThrowIfCancellationRequested();
                            }

                            progress.PerformStep(++actualStep, totalSteps, Resource.SaveDocumentDirectoryDocuments_Progress_Step_Message, Path.GetFileName(filePath));

                            var document = documents.SingleOrDefault(_ => string.Equals(_.Path, filePath, StringComparison.CurrentCultureIgnoreCase));
                            var physicalFileLastWriteTime = File.GetLastWriteTime(filePath);

                            // If database document exists and last write time is equals to the physical file last write time
                            // Ignores and move next.
                            if (document != null && document.LastWriteTime == physicalFileLastWriteTime) { continue; }

                            // If database document exists and last write time is NOT equals to the physical file last write time
                            // Updates the database document.
                            if (document != null && document.LastWriteTime != physicalFileLastWriteTime) {
                                document.Content = ReadFileContent(filePath);
                                document.Payload = ReadFileAsRtf(filePath);
                                document.LastWriteTime = physicalFileLastWriteTime;
                                document.Index = false;
                            }

                            // If database document does not exists, create it.
                            if (document == null) {
                                document = new Document {
                                    DocumentDirectoryID = documentDirectory.DocumentDirectoryID,
                                    Code = Common.ExtractCodeFromDocumentFilePath(filePath),
                                    Content = ReadFileContent(filePath),
                                    Payload = ReadFileAsRtf(filePath),
                                    Path = filePath,
                                    LastWriteTime = physicalFileLastWriteTime,
                                    Index = false
                                };
                            }

                            // Persists the document.
                            SaveDocument(documentDirectory.DocumentDirectoryID, document);
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

        #endregion ICommandHandler<SaveDocumentDirectoryDocumentsCommand> Members
    }
}