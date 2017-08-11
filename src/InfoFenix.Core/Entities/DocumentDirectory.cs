using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using InfoFenix.Core.Data;

namespace InfoFenix.Core.Entities {

    public sealed class DocumentDirectory {

        #region Public Properties

        public int DocumentDirectoryID { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public string Label { get; set; }
        [Required]
        public string Path { get; set; }
        [Required]
        public int Position { get; set; }

        #endregion Public Properties

        #region Public Static Methods

        public static DocumentDirectory Map(IDataReader reader) {
            return new DocumentDirectory {
                DocumentDirectoryID = reader.GetInt32OrDefault(Common.DatabaseSchema.DocumentDirectories.DocumentDirectoryID),
                Label = reader.GetStringOrDefault(Common.DatabaseSchema.DocumentDirectories.Label),
                Path = reader.GetStringOrDefault(Common.DatabaseSchema.DocumentDirectories.Path),
                Code = reader.GetStringOrDefault(Common.DatabaseSchema.DocumentDirectories.Code),
                Position = reader.GetInt32OrDefault(Common.DatabaseSchema.DocumentDirectories.Position)
            };
        }

        #endregion Public Static Methods

        #region Public Methods

        public bool Equals(DocumentDirectory obj) {
            return obj != null && string.Equals(obj.Code, Code, StringComparison.CurrentCulture);
        }

        #endregion Public Methods

        #region Public Override Methods

        public override bool Equals(object obj) {
            return Equals(obj as DocumentDirectory);
        }

        public override int GetHashCode() {
            return Code != null ? Code.GetHashCode() : 0;
        }

        #endregion Public Override Methods
    }
}