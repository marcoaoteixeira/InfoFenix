using System.Collections.Generic;

namespace InfoFenix.Core.Dto {

    public sealed class SearchDto {

        #region Public Properties

        public string QueryTerm { get; set; }

        public IList<IndexDto> Indexes { get; } = new List<IndexDto>();

        #endregion Public Properties
    }
}