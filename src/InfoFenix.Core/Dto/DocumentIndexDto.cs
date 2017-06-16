using System.Text;
using InfoFenix.Core.Search;

namespace InfoFenix.Core.Dto {

    public class DocumentIndexDto {

        #region Public Properties

        public int DocumentID { get; set; }
        public int DocumentCode { get; set; }
        public string DocumentDirectoryCode { get; set; }
        public string Content { get; set; }

        #endregion Public Properties

        #region Public Methods

        public IDocumentIndex Map() {
            return new DocumentIndex(DocumentID.ToString())
                .Add(Common.Index.DocumentFieldName.DocumentCode, DocumentCode).Store()
                .Add(Common.Index.DocumentFieldName.DocumentDirectoryCode, DocumentDirectoryCode).Store()
                .Add(Common.Index.DocumentFieldName.Content, Content).Analyze();
        }

        #endregion Public Methods

        #region Public Static Methods

        public static DocumentIndexDto Map(ISearchHit searchHit) {
            return new DocumentIndexDto {
                DocumentID = int.Parse(searchHit.DocumentID),
                DocumentCode = searchHit.GetInt(Common.Index.DocumentFieldName.DocumentCode),
                DocumentDirectoryCode = searchHit.GetString(Common.Index.DocumentFieldName.DocumentDirectoryCode),
                Content = searchHit.GetString(Common.Index.DocumentFieldName.Content)
            };
        }

        public static DocumentIndexDto Map(DocumentDto documentDto) {
            return new DocumentIndexDto {
                DocumentID = documentDto.DocumentID,
                DocumentCode = documentDto.Code,
                DocumentDirectoryCode = documentDto.DocumentDirectory.Code,
                Content = Encoding.UTF8.GetString(documentDto.Payload)
            };
        }

        #endregion Public Static Methods
    }
}