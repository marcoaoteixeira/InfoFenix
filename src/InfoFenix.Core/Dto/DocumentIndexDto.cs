using System.Text;
using InfoFenix.Core.Search;

namespace InfoFenix.Core.Dto {

    public sealed class DocumentIndexDto {

        #region Public Properties

        public int DocumentID { get; set; }
        public int DocumentCode { get; set; }
        public string DocumentDirectoryCode { get; set; }
        public string FileName { get; set; }
        public string Content { get; set; }

        #endregion Public Properties

        #region Public Methods

        public IDocumentIndex Map() {
            return new DocumentIndex(DocumentID.ToString())
                .Add(Common.DocumentIndex.FieldNames.DocumentCode, DocumentCode).Store()
                .Add(Common.DocumentIndex.FieldNames.DocumentDirectoryCode, DocumentDirectoryCode).Store()
                .Add(Common.DocumentIndex.FieldNames.FileName, FileName).Store()
                .Add(Common.DocumentIndex.FieldNames.Content, Content).Store().Analyze();
        }

        #endregion Public Methods

        #region Public Static Methods

        public static DocumentIndexDto Map(ISearchHit searchHit) {
            return new DocumentIndexDto {
                DocumentID = int.Parse(searchHit.DocumentID),
                DocumentCode = searchHit.GetInt(Common.DocumentIndex.FieldNames.DocumentCode),
                DocumentDirectoryCode = searchHit.GetString(Common.DocumentIndex.FieldNames.DocumentDirectoryCode),
                FileName = searchHit.GetString(Common.DocumentIndex.FieldNames.FileName),
                Content = searchHit.GetString(Common.DocumentIndex.FieldNames.Content)
            };
        }

        public static DocumentIndexDto Map(DocumentDto documentDto) {
            return new DocumentIndexDto {
                DocumentID = documentDto.DocumentID,
                DocumentCode = documentDto.Code,
                DocumentDirectoryCode = documentDto.DocumentDirectory.Code,
                FileName = documentDto.FileName,
                Content = Encoding.UTF8.GetString(documentDto.Payload)
            };
        }

        #endregion Public Static Methods
    }
}