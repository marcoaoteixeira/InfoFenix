using System;
using System.Collections.Generic;
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

    public class ListDocumentsByDocumentDirectoryFromDiskQuery : IQuery<IEnumerable<DocumentDto>> {

        #region Public Properties

        public string DocumentDirectoryPath { get; set; }

        public bool ReadFileContent { get; set; }

        public bool RequireDocumentDirectory { get; set; }

        #endregion Public Properties
    }

    public class ListDocumentsByDocumentDirectoryFromDiskQueryHandler : IQueryHandler<ListDocumentsByDocumentDirectoryFromDiskQuery, IEnumerable<DocumentDto>> {

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

        #endregion

        #region Public Constructors

        public ListDocumentsByDocumentDirectoryFromDiskQueryHandler(IDatabase database, IWordApplication wordApplication) {
            Prevent.ParameterNull(database, nameof(database));
            Prevent.ParameterNull(wordApplication, nameof(wordApplication));
            
            _database = database;
            _wordApplication = wordApplication;
        }

        #endregion Public Constructors

        #region IQueryHandler<ListDocumentsByDocumentDirectoryFromDiskQuery, IEnumerable<DocumentDto>> Members

        public Task<IEnumerable<DocumentDto>> HandleAsync(ListDocumentsByDocumentDirectoryFromDiskQuery query, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => {
                var documentDirectory = query.RequireDocumentDirectory
                    ? _database.ExecuteReaderSingle(Resource.GetDocumentDirectoryByPathSQL, DocumentDirectoryDto.Map, parameters: new[] {
                        Parameter.CreateInputParameter(Common.DatabaseSchema.DocumentDirectories.DocumentDirectoryID, query.DocumentDirectoryPath)
                    })
                    : null;

                var result = new List<DocumentDto>();
                foreach (var filePath in Common.GetDocFiles(query.DocumentDirectoryPath)) {
                    var document = new DocumentDto {
                        Code = Common.ExtractCodeFromFilePath(filePath),
                        DocumentDirectory = documentDirectory,
                        Indexed = false,
                        LastWriteTime = File.GetLastWriteTime(filePath),
                        Path = filePath
                    };
                    if (query.ReadFileContent) {
                        try {
                            using (var wordDocument = _wordApplication.Open(filePath)) {
                                document.Payload = Encoding.UTF8.GetBytes(query.ReadFileContent ? wordDocument.GetText() : string.Empty);
                            }
                        } catch (Exception ex) { Log.Error(ex, $"CANNOT OPEN/READ FILE: {filePath}"); }
                    }
                    result.Add(document);
                }
                return result.AsEnumerable();
            }, cancellationToken);
        }

        #endregion IQueryHandler<ListDocumentsByDocumentDirectoryFromDiskQuery, IEnumerable<DocumentDto>> Members
    }
}