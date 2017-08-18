using System.Collections.Generic;
using System.Linq;

namespace InfoFenix.Dto {

    public sealed class SearchDto {

        #region Public Properties

        public string QueryTerm { get; set; }

        public IList<IndexDto> Indexes { get; set; } = new List<IndexDto>();

        public IndexDto this[string name] {
            get { return Indexes.SingleOrDefault(_ => _.Name == name); }
        }

        #endregion Public Properties
    }
}