using System.Collections.Generic;
using InfoFenix.Core.Entities;

namespace InfoFenix.Core.Dto {

    public class DocumentDirectoryDto {

        #region Public Properties

        public int DocumentDirectoryID { get; set; }
        public string Label { get; set; }
        public string Path { get; set; }
        public string Code { get; set; }
        public bool Watch { get; set; }
        public bool Index { get; set; }

        public IList<DocumentDto> Documents { get; set; } = new List<DocumentDto>();

        #endregion Public Properties

        #region Public Static Methods

        public static DocumentDirectoryDto Map(DocumentDirectoryEntity entity) {
            return new DocumentDirectoryDto {
                DocumentDirectoryID = entity.ID,
                Label = entity.Label,
                Path = entity.Path,
                Code = entity.Code,
                Watch = entity.Watch,
                Index = entity.Index
            };
        }

        #endregion Public Static Methods

        #region Public Methods

        public DocumentDirectoryEntity Map() {
            return new DocumentDirectoryEntity {
                ID = DocumentDirectoryID,
                Label = Label,
                Path = Path,
                Code = Code,
                Watch = Watch,
                Index = Index,
                TotalDocuments = Documents.Count
            };
        }

        #endregion Public Methods
    }
}