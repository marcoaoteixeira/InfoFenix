using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.IO;
using InfoFenix.Core.Data;

namespace InfoFenix.Core.Entities {

    public class DocumentEntity {

        #region Public Properties

        [Required]
        public int ID { get; set; }

        public int DocumentDirectoryID { get; set; }

        [Required]
        [StringLength(6)]
        public string Path { get; set; }

        [Required]
        public DateTime LastWriteTime { get; set; }

        public int Code { get; set; }

        public bool Indexed { get; set; }

        [Required]
        public byte[] Payload { get; set; }

        // Computed
        public string FileName {
            get { return Path != null ? System.IO.Path.GetFileName(Path) : string.Empty; }
        }

        #endregion Public Properties

        #region Public Static Methods

        public static DocumentEntity MapFromDataReader(IDataReader reader) {
            return new DocumentEntity {
                ID = reader.GetInt32OrDefault("id"),
                DocumentDirectoryID = reader.GetInt32OrDefault("document_directory_id"),
                Path = reader.GetStringOrDefault("path"),
                Code = reader.GetInt32OrDefault("code"),
                LastWriteTime = reader.GetDateTimeOrDefault("last_write_time", DateTime.MinValue),
                Indexed = reader.GetInt32OrDefault("indexed") > 0,
                Payload = reader.GetBlobOrDefault("payload")
            };
        }

        #endregion Public Static Methods

        #region Public Methods

        public bool Equals(DocumentEntity obj) {
            return obj != null && obj.ID == ID;
        }

        #endregion Public Methods

        #region Public Override Methods

        public override bool Equals(object obj) {
            return Equals(obj as DocumentEntity);
        }

        public override int GetHashCode() {
            return ID.GetHashCode();
        }

        #endregion Public Override Methods
    }
}