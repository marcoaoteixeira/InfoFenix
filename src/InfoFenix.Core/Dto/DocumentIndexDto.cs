using InfoFenix.Search;

namespace InfoFenix.Dto {

    public sealed class DocumentIndexDto {

        #region Public Properties

        public int DocumentID { get; set; }
        public long DocumentCode { get; set; }
        public string DocumentDirectoryCode { get; set; }
        public string Content { get; set; }
        public string FileName { get; set; }

        #endregion Public Properties

        #region Public Static Methods

        public static DocumentIndexDto Map(ISearchHit searchHit) => new DocumentIndexDto {
            DocumentID = int.Parse(searchHit.DocumentID),
            DocumentCode = searchHit.GetInt(Common.DocumentIndex.FieldNames.DocumentCode),
            DocumentDirectoryCode = searchHit.GetString(Common.DocumentIndex.FieldNames.DocumentDirectoryCode),
            Content = searchHit.GetString(Common.DocumentIndex.FieldNames.Content),
            FileName = searchHit.GetString(Common.DocumentIndex.FieldNames.FileName)
        };

        #endregion Public Static Methods
    }
}