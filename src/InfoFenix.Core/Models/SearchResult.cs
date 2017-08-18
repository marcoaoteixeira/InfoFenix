using System.Linq;
using InfoFenix.Dto;

namespace InfoFenix.Models {

    public sealed class SearchResult {

        #region Public Properties

        public string[] Terms { get; set; } = Enumerable.Empty<string>().ToArray();

        public DocumentIndexDto[] Documents { get; set; } = Enumerable.Empty<DocumentIndexDto>().ToArray();

        #endregion Public Properties
    }
}