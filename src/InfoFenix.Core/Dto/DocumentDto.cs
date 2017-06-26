using System;
using System.Data;
using InfoFenix.Core.Data;

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
            get { return Path != null ? System.IO.Path.GetFileName(Path) : null; }
        }

        public DocumentDirectoryDto DocumentDirectory { get; set; }

        #endregion Public Properties

        #region Public Static Methods

        public static DocumentDto Map(IDataReader reader) {
            return Map(reader, documentDirectory: null);
        }

        public static DocumentDto Map(IDataReader reader, DocumentDirectoryDto documentDirectory) {
            return new DocumentDto {
                DocumentID = reader.GetInt32OrDefault(Common.DatabaseSchema.Documents.DocumentID),
                DocumentDirectory = documentDirectory ?? new DocumentDirectoryDto {
                    DocumentDirectoryID = reader.GetInt32OrDefault(Common.DatabaseSchema.Documents.DocumentDirectoryID)
                },
                Path = reader.GetStringOrDefault(Common.DatabaseSchema.Documents.Path),
                Code = reader.GetInt32OrDefault(Common.DatabaseSchema.Documents.Code),
                LastWriteTime = reader.GetDateTimeOrDefault(Common.DatabaseSchema.Documents.LastWriteTime, DateTime.MinValue),
                Indexed = reader.GetInt32OrDefault(Common.DatabaseSchema.Documents.Indexed) > 0,
                Payload = reader.GetBlobOrDefault(Common.DatabaseSchema.Documents.Payload)
            };
        }

        #endregion Public Static Methods

        #region Public Methods

        public bool Equals(DocumentDto obj) {
            return obj != null && obj.DocumentID == DocumentID;
        }

        #endregion Public Methods

        #region Public Override Methods

        public override bool Equals(object obj) {
            return Equals(obj as DocumentDto);
        }

        public override int GetHashCode() {
            return DocumentID.GetHashCode();
        }

        #endregion Public Override Methods
    }
}