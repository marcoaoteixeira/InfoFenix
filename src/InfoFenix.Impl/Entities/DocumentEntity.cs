using System;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace InfoFenix.Core.Entities {

    public class DocumentEntity {

        #region Public Properties

        [Required]
        public int DocumentID { get; set; }
        
        public int DocumentDirectoryID { get; set; }
        
        [Required]
        [StringLength(6)]
        public string FullPath { get; set; }

        public string FileName {
            get { return FullPath != null ? Path.GetFileName(FullPath) : string.Empty; }
        }
        
        [Required]
        public DateTime LastWriteTime { get; set; }

        public int Code { get; set; }

        public bool Indexed { get; set; }

        [Required]
        public byte[] Payload { get; set; }

        #endregion Public Properties

        #region Public Methods

        public bool Equals(DocumentEntity obj) {
            return obj != null && obj.DocumentID == DocumentID;
        }

        #endregion Public Methods

        #region Public Override Methods

        public override bool Equals(object obj) {
            return Equals(obj as DocumentEntity);
        }

        public override int GetHashCode() {
            return DocumentID.GetHashCode();
        }

        #endregion Public Override Methods
    }
}