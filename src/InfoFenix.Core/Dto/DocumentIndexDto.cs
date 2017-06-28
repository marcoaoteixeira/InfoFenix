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
                .Set(Common.DocumentIndex.FieldNames.DocumentCode, DocumentCode, DocumentIndexOptions.Store)
                .Set(Common.DocumentIndex.FieldNames.DocumentDirectoryCode, DocumentDirectoryCode, DocumentIndexOptions.Store)
                .Set(Common.DocumentIndex.FieldNames.FileName, FileName, DocumentIndexOptions.Store)
                .Set(Common.DocumentIndex.FieldNames.Content, Content, DocumentIndexOptions.Analyze | DocumentIndexOptions.Store);
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
                Content = Encoding.UTF8.GetString(documentDto.Payload ?? new byte[0])
            };
        }

        #endregion Public Static Methods
    }
}