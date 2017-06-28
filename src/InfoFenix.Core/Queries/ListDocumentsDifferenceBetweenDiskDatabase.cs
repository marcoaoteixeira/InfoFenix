using System;
using System.Collections.Generic;
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

namespace InfoFenix.Core.Queries {

    public class ListDocumentsDifferenceBetweenDiskDatabaseQuery : IQuery<IEnumerable<DocumentDto>> {

        #region Public Properties

        public int DocumentDirectoryID { get; set; }

        #endregion Public Properties
    }

    public class ListDocumentsDifferenceBetweenDiskDatabaseQueryHandler : IQueryHandler<ListDocumentsDifferenceBetweenDiskDatabaseQuery, IEnumerable<DocumentDto>> {

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

        public ListDocumentsDifferenceBetweenDiskDatabaseQueryHandler(IDatabase database, IWordApplication wordApplication) {
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

        #endregion Private Methods

        #region Private Inner Classes 

        private class Document {
            #region Public Properties

            public DocumentDto Current { get; set; }
            public bool Save { get; set; }

            #endregion

            #region Public Constructors

            public Document(DocumentDto current, bool save) {
                Current = current;
                Save = save;
            }

            #endregion

            #region Public Static Methods

            public static Document Create(DocumentDto current, bool save) {
                return new Document(current, save);
            }

            #endregion
        }

        #endregion

        #region IQueryHandler<ListDocumentsDifferenceBetweenDiskDatabaseQuery, IEnumerable<DocumentDto>> Members

        public Task<IEnumerable<DocumentDto>> HandleAsync(ListDocumentsDifferenceBetweenDiskDatabaseQuery query, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => {
                var documentDirectory = _database
                    .ExecuteReaderSingle(Resource.GetDocumentDirectorySQL, DocumentDirectoryDto.Map, parameters: new[] {
                        Parameter.CreateInputParameter(Common.DatabaseSchema.DocumentDirectories.DocumentDirectoryID, query.DocumentDirectoryID, DbType.Int32)
                    });
                var documents = _database
                    .ExecuteReader(Resource.ListDocumentsByDocumentDirectorySQL, (reader) => DocumentDto.Map(reader, documentDirectory), parameters: new[] {
                        Parameter.CreateInputParameter(Common.DatabaseSchema.Documents.DocumentDirectoryID, documentDirectory.DocumentDirectoryID, DbType.Int32)
                    })
                    .Select(_ => Document.Create(current: _, save: false))
                    .ToList();

                foreach (var filePath in Common.GetDocFiles(documentDirectory.Path)) {
                    cancellationToken.ThrowIfCancellationRequested();

                    var document = documents.SingleOrDefault(_ => string.Equals(_.Current.Path, filePath, StringComparison.CurrentCultureIgnoreCase));
                    var physicalFileLastWriteTime = File.GetLastWriteTime(filePath);

                    // If database document exists and last write time is equals to the physical file last write time
                    // Ignores and move next.
                    if (document != null && document.Current.LastWriteTime == physicalFileLastWriteTime) {
                        document.Save = false;

                        continue;
                    }

                    // If database document exists and last write time is NOT equals to the physical file last write time
                    // Updates the database document.
                    if (document != null && document.Current.LastWriteTime != physicalFileLastWriteTime) {
                        document.Current.Indexed = false;
                        document.Current.LastWriteTime = physicalFileLastWriteTime;
                        document.Current.Payload = ReadFileContent(filePath);
                        document.Save = true;

                        continue;
                    }

                    // If database document does not exists, create it.
                    if (document == null) {
                        documents.Add(Document.Create(new DocumentDto {
                            Code = Common.ExtractCodeFromFilePath(filePath),
                            DocumentDirectory = documentDirectory,
                            DocumentID = 0,
                            Indexed = false,
                            LastWriteTime = physicalFileLastWriteTime,
                            Path = filePath,
                            Payload = ReadFileContent(filePath)
                        }, save: true));
                    }
                }

                return documents.Where(_ => _.Save).Select(_ => _.Current);
            }, cancellationToken);
        }

        #endregion IQueryHandler<ListDocumentsDifferenceBetweenDiskDatabaseQuery, IEnumerable<DocumentDto>> Members
    }
}