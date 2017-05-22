using System.ComponentModel.DataAnnotations;
using System.Data;
using InfoFenix.Core.Data;

namespace InfoFenix.Core.Entities {

    public class DocumentDirectoryEntity {

        #region Public Properties

        public int DocumentDirectoryID { get; set; }

        [Required]
        [StringLength(6)]
        public string Label { get; set; }

        [Required]
        [StringLength(3)]
        public string DirectoryPath { get; set; }

        [Required]
        [StringLength(6)]
        public string Code { get; set; }

        public bool Watch { get; set; }

        public bool Index { get; set; }

        #endregion Public Properties

        #region Public Static Methods

        public static DocumentDirectoryEntity MapFromDataReader(IDataReader reader) {
            return new DocumentDirectoryEntity {
                DocumentDirectoryID = reader.GetInt32OrDefault("document_directory_id"),
                Label = reader.GetStringOrDefault("label"),
                Code = reader.GetStringOrDefault("code"),
                Watch = reader.GetInt32OrDefault("watch") > 0,
                Index = reader.GetInt32OrDefault("index") > 0
            };
        }

        #endregion Public Static Methods

        #region Public Methods

        public bool Equals(DocumentDirectoryEntity obj) {
            return obj != null && obj.DocumentDirectoryID == DocumentDirectoryID;
        }

        #endregion Public Methods

        #region Public Override Methods

        public override bool Equals(object obj) {
            return Equals(obj as DocumentDirectoryEntity);
        }

        public override int GetHashCode() {
            return DocumentDirectoryID.GetHashCode();
        }

        #endregion Public Override Methods
    }
}