using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InfoFenix.CQRS;
using InfoFenix.Data;
using InfoFenix.Entities;
using InfoFenix.Logging;
using InfoFenix.Resources;
using InfoFenix.Search;

namespace InfoFenix.Domains.Commands {

    public sealed class CleanDocumentDirectoryCommand : ICommand {

        #region Public Properties

        public int DocumentDirectoryID { get; set; }

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
                    var documentDirectory = _database.ExecuteReaderSingle(SQLs.GetDocumentDirectory, DocumentDirectory.Map, parameters: new[] {
                        Parameter.CreateInputParameter(Common.DatabaseSchema.DocumentDirectories.DocumentDirectoryID, command.DocumentDirectoryID, DbType.Int32)
                    });
                    var documents = _database.ExecuteReader(SQLs.ListDocumentsByDocumentDirectoryNoContent, Document.Map, parameters: new[] {
                        Parameter.CreateInputParameter(Common.DatabaseSchema.DocumentDirectories.DocumentDirectoryID, command.DocumentDirectoryID, DbType.Int32)
                    });

                    var filesOnDatabase = documents.Select(_ => _.Path).ToArray();
                    var filesOnDisk = Common.GetDocumentFiles(documentDirectory.Path);
                    var removeFromDatabase = filesOnDatabase.Except(filesOnDisk).ToArray();
                    var removeFromIndex = new List<Document>();

                    totalSteps = removeFromDatabase.Length + 1;

                    progress.Start(totalSteps, Strings.CleanDocumentDirectory_Progress_Start_Title);

                    using (var transaction = _database.Connection.BeginTransaction()) {
                        foreach (var filePath in removeFromDatabase) {
                            progress.PerformStep(++actualStep, totalSteps, Strings.CleanDocumentDirectory_Progress_Step_Database_Message, Path.GetFileName(filePath));

                            if (cancellationToken.IsCancellationRequested) {
                                progress.Cancel(actualStep, totalSteps);

                                cancellationToken.ThrowIfCancellationRequested();
                            }

                            var document = documents.SingleOrDefault(_ => _.Path == filePath);
                            _database.ExecuteNonQuery(SQLs.RemoveDocument, parameters: new[] {
                                Parameter.CreateInputParameter(Common.DatabaseSchema.Documents.DocumentID, document.DocumentID, DbType.Int32)
                            });

                            removeFromIndex.Add(document);
                        }

                        if (cancellationToken.IsCancellationRequested) {
                            progress.Cancel(actualStep, totalSteps);

                            cancellationToken.ThrowIfCancellationRequested();
                        }
                        transaction.Commit();
                    }

                    progress.PerformStep(++actualStep, totalSteps, Strings.CleanDocumentDirectory_Progress_Step_Index_Message, documentDirectory.Label);
                    _indexProvider
                        .GetOrCreate(documentDirectory.Code)
                        .DeleteDocuments(removeFromIndex.Select(_ => _.DocumentID.ToString()).ToArray());

                    progress.Complete(actualStep, totalSteps);
                } catch (Exception ex) { progress.Error(actualStep, totalSteps, ex.Message); throw; }
            }, cancellationToken);
        }

        #endregion ICommandHandler<CleanDocumentDirectoryCommand> Members
    }
}