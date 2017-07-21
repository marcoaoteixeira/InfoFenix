using System;
using System.IO;
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

    public class GetDocumentDirectoryContentChangeQuery : IQuery<DocumentDto> {

        #region Public Properties

        public string DocumentDirectoryPath { get; set; }
        public string DocumentPath { get; set; }

        #endregion Public Properties
    }

    public class GetDocumentDirectoryContentChangeQueryHandler : IQueryHandler<GetDocumentDirectoryContentChangeQuery, DocumentDto> {

        #region Private Static Read-Only Fields

        private static readonly string TempFilePath = Path.Combine(Path.GetTempPath(), Path.GetTempFileName());

        #endregion

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

        public GetDocumentDirectoryContentChangeQueryHandler(IDatabase database, IWordApplication wordApplication) {
            Prevent.ParameterNull(database, nameof(database));
            Prevent.ParameterNull(wordApplication, nameof(wordApplication));

            _database = database;
            _wordApplication = wordApplication;
        }

        #endregion Public Constructors

        #region IQueryHandler<GetDocumentDirectoryContentChangeQuery, DocumentDto> Members

        public Task<DocumentDto> HandleAsync(GetDocumentDirectoryContentChangeQuery query, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => {
                cancellationToken.ThrowIfCancellationRequested();
                var documentDirectory = _database.ExecuteReaderSingle(Resource.GetDocumentDirectoryByPathSQL, DocumentDirectoryDto.Map, parameters: new[] {
                    Parameter.CreateInputParameter(Common.DatabaseSchema.DocumentDirectories.Path, query.DocumentDirectoryPath)
                });
                var document = _database.ExecuteReaderSingle(Resource.GetDocumentByPathSQL, (reader) => DocumentDto.Map(reader, documentDirectory), parameters: new[] {
                    Parameter.CreateInputParameter(Common.DatabaseSchema.Documents.Path, query.DocumentPath)
                });

                if (document == null) {
                    document = new DocumentDto {
                        Code = Common.ExtractCodeFromFilePath(query.DocumentPath),
                        Path = query.DocumentPath,

                        DocumentDirectory = documentDirectory
                    };
                }
                document.Indexed = false;
                document.LastWriteTime = File.GetLastWriteTime(query.DocumentPath);

                cancellationToken.ThrowIfCancellationRequested();
                var content = string.Empty;
                byte[] payload = null;
                try {
                    using (var wordDocument = _wordApplication.Open(query.DocumentPath)) {
                        content = wordDocument.GetText();
                        wordDocument.SaveAs(TempFilePath);
                    }

                    payload = File.ReadAllBytes(TempFilePath);
                }
                catch (Exception ex) { Log.Error(ex, "CANNOT OPEN/READ DOCUMENT: {0}", query.DocumentPath); throw; }
                document.Content = content;
                document.Payload = payload;

                return document;
            }, cancellationToken);
        }

        #endregion IQueryHandler<GetDocumentDirectoryContentChangeQuery, DocumentDto> Members
    }
}