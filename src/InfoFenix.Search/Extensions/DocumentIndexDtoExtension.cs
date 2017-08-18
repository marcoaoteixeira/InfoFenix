using InfoFenix.Dto;

namespace InfoFenix.Search {

    public static class DocumentIndexDtoExtension {

        #region Public Static Class

        public static IDocumentIndex Map(this DocumentIndexDto source) {
            if (source == null) { return null; }
            return new DocumentIndex(source.DocumentID.ToString())
                .Set(Common.DocumentIndex.FieldNames.DocumentCode, source.DocumentCode, DocumentIndexOptions.Store)
                .Set(Common.DocumentIndex.FieldNames.DocumentDirectoryCode, source.DocumentDirectoryCode, DocumentIndexOptions.Store)
                .Set(Common.DocumentIndex.FieldNames.Content, source.Content ?? string.Empty, DocumentIndexOptions.Analyze | DocumentIndexOptions.Store)
                .Set(Common.DocumentIndex.FieldNames.FileName, source.FileName, DocumentIndexOptions.Store);
        }

        #endregion Public Static Class
    }
}