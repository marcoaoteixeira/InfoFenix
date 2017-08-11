using System;
using System.Data;
using System.Diagnostics;
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

        public int BatchSize { get; set; } = 128;

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

        private DocumentTransform Transform(string filePath) {
            var result = new DocumentTransform();
            try {
                using (var wordDocument = _wordApplication.Open(filePath)) {
                    result.Content = wordDocument.GetText();
                    wordDocument.SaveAs(TempFilePath);
                    result.Payload = File.ReadAllBytes(TempFilePath);
                    File.Delete(TempFilePath);
                }
            } catch { Log.Error($"CANNOT SAVE FILE: {filePath}"); }
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
                var sw = new Stopwatch();

                var documentDirectory = _database.ExecuteReaderSingle(Resource.GetDocumentDirectorySQL, DocumentDirectory.Map, parameters: new[] {
                    Parameter.CreateInputParameter(Common.DatabaseSchema.DocumentDirectories.DocumentDirectoryID, command.DocumentDirectoryID, DbType.Int32)
                });

                var documents = _database.ExecuteReader(Resource.ListDocumentsByDocumentDirectoryNoContentSQL, Document.Map, parameters: new[] {
                    Parameter.CreateInputParameter(Common.DatabaseSchema.DocumentDirectories.DocumentDirectoryID, command.DocumentDirectoryID, DbType.Int32)
                }).ToArray();

                var physicalFiles = Common.GetDocumentFiles(documentDirectory.Path);
                var actualStep = 0;
                var totalSteps = physicalFiles.Length;

                try {
                    progress.Start(totalSteps, Resource.SaveDocumentDirectoryDocuments_Progress_Start_Title);

                    var pageCount = (physicalFiles.Length / command.BatchSize) + 1;
                    for (var page = 0; page < pageCount; page++) {
                        var chunk = physicalFiles.Skip(page * command.BatchSize).Take(command.BatchSize);
                        using (var transaction = _database.Connection.BeginTransaction()) {
                            foreach (var filePath in chunk) {
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

                                Log.Information("Create temporary document on disk. Original file: {0}", filePath);
                                sw.Start();
                                var documentTransform = Transform(filePath);
                                sw.Stop();
                                Log.Information("Elapsed time: {0}ms", sw.ElapsedMilliseconds);
                                sw.Reset();

                                // If database document exists and last write time is NOT equals to the physical file last write time
                                // Updates the database document.
                                if (document != null && document.LastWriteTime != physicalFileLastWriteTime) {
                                    document.Content = documentTransform.Content;
                                    document.Payload = documentTransform.Payload;
                                    document.LastWriteTime = physicalFileLastWriteTime;
                                    document.Index = false;
                                }

                                // If database document does not exists, create it.
                                if (document == null) {
                                    document = new Document {
                                        DocumentDirectoryID = documentDirectory.DocumentDirectoryID,
                                        Code = Common.ExtractCodeFromDocumentFilePath(filePath),
                                        Content = documentTransform.Content,
                                        Payload = documentTransform.Payload,
                                        Path = filePath,
                                        LastWriteTime = physicalFileLastWriteTime,
                                        Index = false
                                    };
                                }

                                Log.Information("Saving file to database. Original file: {0}", filePath);
                                // Persists the document.
                                sw.Start();
                                SaveDocument(documentDirectory.DocumentDirectoryID, document);
                                sw.Stop();
                                Log.Information("Elapsed time: {0}ms", sw.ElapsedMilliseconds);
                                sw.Reset();
                            }

                            if (cancellationToken.IsCancellationRequested) {
                                progress.Cancel(actualStep, totalSteps);

                                cancellationToken.ThrowIfCancellationRequested();
                            }
                            transaction.Commit();
                        }
                    }
                    progress.Complete(actualStep, totalSteps);
                } catch (Exception ex) { progress.Error(actualStep, totalSteps, ex.Message); throw; }
            }, cancellationToken);
        }

        #endregion ICommandHandler<SaveDocumentDirectoryDocumentsCommand> Members

        #region Private Inner Classes

        private class DocumentTransform {

            #region Public Properties

            public string Content { get; set; }
            public byte[] Payload { get; set; }

            #endregion Public Properties
        }

        #endregion Private Inner Classes
    }
}