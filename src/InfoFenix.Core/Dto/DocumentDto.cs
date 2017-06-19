using System;
using System.Data;
using InfoFenix.Core.Entities;

namespace InfoFenix.Core.Dto {

    public class DocumentDto {

        #region Public Properties

        public int DocumentID { get; set; }
        public string Path { get; set; }
        public DateTime LastWriteTime { get; set; }
        public int Code { get; set; }
        public bool Indexed { get; set; }
        public byte[] Payload { get; set; }

        public string FileName {
            get { return Path != null ? System.IO.Path.GetFileNameWithoutExtension(Path) : null; }
        }

        public DocumentDirectoryDto DocumentDirectory { get; set; }

        #endregion Public Properties

        #region Public Static Methods

        public static DocumentDto Map(DocumentEntity entity) {
            return new DocumentDto {
                DocumentID = entity.ID,
                Path = entity.Path,
                LastWriteTime = entity.LastWriteTime,
                Code = entity.Code,
                Indexed = entity.Indexed,
                Payload = entity.Payload,
                DocumentDirectory = new DocumentDirectoryDto {
                    DocumentDirectoryID = entity.DocumentDirectoryID
                }
            };
        }

        public static DocumentDto Map(IDataReader reader) {
            return Map(DocumentEntity.MapFromDataReader(reader));
        }

        #endregion Public Static Methods

        #region Public Methods

        public DocumentEntity Map() {
            return new DocumentEntity {
                ID = DocumentID,
                Path = Path,
                LastWriteTime = LastWriteTime,
                Code = Code,
                Indexed = Indexed,
                Payload = Payload,
                DocumentDirectoryID = DocumentDirectory != null ? DocumentDirectory.DocumentDirectoryID : 0
            };
        }

        #endregion Public Methods
    }
}