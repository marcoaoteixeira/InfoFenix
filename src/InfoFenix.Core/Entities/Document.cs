using System;
using System.Data;
using System.IO;
using InfoFenix.Core.Data;

namespace InfoFenix.Core.Entities {

    public sealed class Document {

        #region Public Properties

        public int DocumentID { get; set; }
        public int DocumentDirectoryID { get; set; }
        public int Code { get; set; }
        public string Content { get; set; }
        public byte[] Payload { get; set; }
        public string Path { get; set; }
        public DateTime LastWriteTime { get; set; }
        public bool Index { get; set; }

        public string FileName {
            get { return Path != null ? System.IO.Path.GetFileName(Path) : null; }
        }

        #endregion Public Properties

        #region Public Static Methods

        public static Document Map(string filePath) {
            return new Document {
                Code = Common.ExtractCodeFromDocumentFilePath(filePath),
                Payload = File.ReadAllBytes(filePath),
                Path = filePath
            };
        }

        public static Document Map(IDataReader reader) {
            return new Document {
                DocumentID = reader.GetInt32OrDefault(Common.DatabaseSchema.Documents.DocumentID),
                DocumentDirectoryID = reader.GetInt32OrDefault(Common.DatabaseSchema.Documents.DocumentDirectoryID),
                Code = reader.GetInt32OrDefault(Common.DatabaseSchema.Documents.Code),
                Content = reader.GetStringOrDefault(Common.DatabaseSchema.Documents.Content),
                Payload = reader.GetBlobOrDefault(Common.DatabaseSchema.Documents.Payload),
                Path = reader.GetStringOrDefault(Common.DatabaseSchema.Documents.Path),
                LastWriteTime = reader.GetDateTimeOrDefault(Common.DatabaseSchema.Documents.LastWriteTime, DateTime.MinValue),
                Index = reader.GetInt32OrDefault(Common.DatabaseSchema.Documents.Index) > 0,
            };
        }

        #endregion Public Static Methods

        #region Public Methods

        public bool Equals(Document obj) {
            return obj != null
                && obj.DocumentDirectoryID == DocumentDirectoryID
                && string.Equals(obj.FileName, FileName, StringComparison.CurrentCultureIgnoreCase);
        }

        #endregion Public Methods

        #region Public Override Methods

        public override bool Equals(object obj) {
            return Equals(obj as Document);
        }

        public override int GetHashCode() {
            var hash = 13;
            unchecked {
                hash += DocumentDirectoryID.GetHashCode() * 7;
                hash += (FileName ?? string.Empty).GetHashCode() * 7;
            }
            return hash;
        }

        #endregion Public Override Methods
    }
}