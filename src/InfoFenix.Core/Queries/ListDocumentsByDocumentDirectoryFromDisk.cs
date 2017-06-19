using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using InfoFenix.Core.Cqrs;
using InfoFenix.Core.Dto;
using InfoFenix.Core.Office;

namespace InfoFenix.Core.Queries {

    public class ListDocumentsByDocumentDirectoryFromDiskQuery : IQuery<IEnumerable<DocumentDto>> {

        #region Public Properties

        public string DocumentDirectoryPath { get; set; }

        public bool ReadFileContent { get; set; }

        #endregion Public Properties
    }

    public class ListDocumentsByDocumentDirectoryFromDiskQueryHandler : IQueryHandler<ListDocumentsByDocumentDirectoryFromDiskQuery, IEnumerable<DocumentDto>> {

        #region Private Read-Only Fields

        private readonly IWordApplication _wordApplication;

        #endregion Private Read-Only Fields

        #region Public Constructors

        public ListDocumentsByDocumentDirectoryFromDiskQueryHandler(IWordApplication wordApplication) {
            Prevent.ParameterNull(wordApplication, nameof(wordApplication));

            _wordApplication = wordApplication;
        }

        #endregion Public Constructors

        #region IQueryHandler<ListDocumentsByDocumentDirectoryFromDiskQuery, IEnumerable<DocumentDto>> Members

        public Task<IEnumerable<DocumentDto>> HandleAsync(ListDocumentsByDocumentDirectoryFromDiskQuery query, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => {
                var result = new List<DocumentDto>();
                foreach (var file in Common.GetDocFiles(query.DocumentDirectoryPath)) {
                    var document = new DocumentDto {
                        Code = Common.ExtractCodeFromFilePath(file),
                        Path = file,
                        LastWriteTime = File.GetLastWriteTime(file)
                    };
                    if (query.ReadFileContent) {
                        using (var wordDocument = _wordApplication.Open(file)) {
                            document.Payload = Encoding.UTF8.GetBytes(query.ReadFileContent ? wordDocument.GetText() : string.Empty);
                        }
                    }
                    result.Add(document);
                }
                return result.AsEnumerable();
            }, cancellationToken);
        }

        #endregion IQueryHandler<ListDocumentsByDocumentDirectoryFromDiskQuery, IEnumerable<DocumentDto>> Members
    }
}