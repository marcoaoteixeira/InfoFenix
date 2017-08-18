using System;
using System.Data;
using InfoFenix.Data;

namespace InfoFenix.Migrations {

    public sealed class Migration {

        #region Public Properties

        public string Version { get; set; }
        public DateTime Date { get; set; }

        #endregion Public Properties

        #region Public Static Methods
        
        public static Migration Map(IDataReader reader) {
            return new Migration {
                Version = reader.GetStringOrDefault(Common.DatabaseSchema.Migrations.Version),
                Date = reader.GetDateTimeOrDefault(Common.DatabaseSchema.Migrations.Date, DateTime.MinValue)
            };
        }

        #endregion Public Static Methods

        #region Public Methods

        public bool Equals(Migration obj) {
            return obj != null && obj.Version == Version;
        }

        #endregion Public Methods

        #region Public Override Methods

        public override bool Equals(object obj) {
            return Equals(obj as Migration);
        }

        public override int GetHashCode() {
            var hash = 13;
            unchecked {
                hash += (Version ?? string.Empty).GetHashCode() * 7;
            }
            return hash;
        }

        #endregion Public Override Methods
    }
}