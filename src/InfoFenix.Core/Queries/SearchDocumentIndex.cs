using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InfoFenix.Core.Cqrs;
using InfoFenix.Core.Dto;
using InfoFenix.Core.Search;

namespace InfoFenix.Core.Queries {

    public class SearchDocumentIndexResult {

        #region Public Properties

        public string IndexName { get; set; }

        public string Label { get; set; }

        public int TotalDocuments { get; set; }

        public IList<SearchDocumentIndexResultItem> Documents { get; set; } = new List<SearchDocumentIndexResultItem>();

        #endregion Public Properties
    }

    public class SearchDocumentIndexResultItem : Expando {

    }

    public class SearchDocumentIndexQuery : IQuery<IEnumerable<SearchDocumentIndexResult>> {

        #region Public Properties

        public string Term { get; set; }

        public ISet<IndexDto> Indexes { get; set; } = new HashSet<IndexDto>();

        public IndexDto this[string name] {
            get { return Indexes?.SingleOrDefault(_ => _.Name == name); }
        }
        
        #endregion Public Properties
    }

    public class SearchDocumentIndexQueryHandler : IQueryHandler<SearchDocumentIndexQuery, IEnumerable<SearchDocumentIndexResult>> {

        #region Private Read-Only Fields

        private readonly IIndexProvider _indexProvider;

        #endregion Private Read-Only Fields

        #region Public Constructors

        public SearchDocumentIndexQueryHandler(IIndexProvider indexProvider) {
            Prevent.ParameterNull(indexProvider, nameof(indexProvider));

            _indexProvider = indexProvider;
        }

        #endregion Public Constructors

        #region Private Static Methods

        private static ISearchBuilder CreateSearchBuilder(IIndex index, string[] positiveTerms, string[] negativeTerms) {
            var searchBuilder = index.CreateSearchBuilder();
            // Search positive terms
            foreach (var item in positiveTerms) {
                if (item.IndexOf("-") > 0 || item.IndexOf("-") == (item.Length - 1)) {
                    searchBuilder = searchBuilder
                        .WithField(Common.Index.DocumentFieldName.Content, item, item.Contains("*"))
                        .NotAnalyzed();
                } else {
                    searchBuilder = searchBuilder
                        .WithField(Common.Index.DocumentFieldName.Content, item, item.Contains("*"))
                        .ExactMatch()
                        .Mandatory();
                }
            }
            // Search negative terms
            foreach (var item in negativeTerms) {
                searchBuilder = searchBuilder
                    .WithField(Common.Index.DocumentFieldName.Content, item, false)
                    .Forbidden();
            }

            searchBuilder
                .SortByInteger(Common.Index.DocumentFieldName.DocumentCode)
                .Ascending();

            return searchBuilder;
        }

        #endregion Private Static Methods

        #region Private Methods

        private IEnumerable<IIndex> GetIndexes(IEnumerable<string> indexNames) {
            if (indexNames.IsNullOrEmpty()) { indexNames = _indexProvider.List(); }
            foreach (var indexName in indexNames) {
                yield return _indexProvider.GetOrCreate(indexName);
            }
        }

        private SearchDocumentIndexResult ExecuteSearch(IIndex index, ISearchBuilder searchBuilder, string label) {
            var searchIndexResult = new SearchDocumentIndexResult {
                IndexName = index.Name,
                Label = label,
                TotalDocuments = index.TotalDocuments()
            };
            foreach (var searchHit in searchBuilder.Search()) {
                dynamic item = new SearchDocumentIndexResultItem();
                item.ID = searchHit.DocumentID;
                item.DocumentCode = searchHit.GetInt(Common.Index.DocumentFieldName.DocumentCode);
                item.DocumentDirectoryCode = searchHit.GetString(Common.Index.DocumentFieldName.DocumentDirectoryCode);
                item.Content = searchHit.GetString(Common.Index.DocumentFieldName.Content);
                searchIndexResult.Documents.Add(item);
            }
            return searchIndexResult;
        }

        #endregion Private Methods

        #region IQueryHandler<SearchDocumentIndexQuery, IEnumerable<SearchDocumentIndexResult>> Members

        public Task<IEnumerable<SearchDocumentIndexResult>> HandleAsync(SearchDocumentIndexQuery query, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => {
                if (string.IsNullOrWhiteSpace(query.Term)) { return Enumerable.Empty<SearchDocumentIndexResult>(); }

                var criterions = query.Term.Split(' ');
                var positiveTerms = criterions.Where(_ => !_.StartsWith("-")).ToArray();
                var negativeTerms = criterions.Except(positiveTerms).ToArray();

                var result = new List<SearchDocumentIndexResult>();
                foreach (var index in GetIndexes(query.Indexes.Select(_ => _.Name))) {
                    if (cancellationToken.IsCancellationRequested) { break; }
                    var searchBuilder = CreateSearchBuilder(index, positiveTerms, negativeTerms);
                    var searchIndexResult = ExecuteSearch(index, searchBuilder, query[index.Name].Label);
                    result.Add(searchIndexResult);
                }
                return result;
            }, cancellationToken);
        }

        #endregion IQueryHandler<SearchDocumentIndexQuery, IEnumerable<SearchDocumentIndexResult>> Members
    }
}