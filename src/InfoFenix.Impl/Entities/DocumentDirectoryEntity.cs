using System.ComponentModel.DataAnnotations;

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