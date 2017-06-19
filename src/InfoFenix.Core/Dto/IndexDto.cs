using System.Collections.Generic;

namespace InfoFenix.Core.Dto {

    public sealed class IndexDto {

        #region Public Properties

        public string Name { get; set; }
        public string Label { get; set; }
        public int TotalDocuments { get; set; }

        public IList<DocumentIndexDto> Documents { get; set; } = new List<DocumentIndexDto>();

        #endregion Public Properties

        #region Public Static Methods

        public static IndexDto New(string name, string label) {
            return new IndexDto {
                Name = name,
                Label = label
            };
        }

        #endregion Public Static Methods

        #region Public Methods

        public bool Equals(IndexDto obj) {
            return obj != null &&
                   obj.Name == Name &&
                   obj.Label == Label;
        }

        #endregion Public Methods

        #region Public Override Methods

        public override bool Equals(object obj) {
            return Equals(obj as IndexDto);
        }

        public override int GetHashCode() {
            var hash = 13;
            unchecked {
                hash += Name != null ? Name.GetHashCode() * 7 : 0;
                hash += Label != null ? Label.GetHashCode() * 7 : 0;
            }
            return hash;
        }

        #endregion Public Override Methods
    }
}