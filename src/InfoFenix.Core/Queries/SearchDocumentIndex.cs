using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InfoFenix.Core.Cqrs;
using InfoFenix.Core.PubSub;
using InfoFenix.Core.Search;

namespace InfoFenix.Core.Queries {

    public class SearchDto {

        #region Public Properties

        public string IndexName { get; set; }

        public string Label { get; set; }

        public int TotalDocuments { get; set; }

        public IList<SearchDocumentIndexResultItem> Documents { get; set; } = new List<SearchDocumentIndexResultItem>();

        #endregion Public Properties
    }

    public class SearchDocumentIndexResultItem : Expando {
    }

    public class SearchDocumentIndexQuery : IQuery<SearchDto> {

        #region Public Properties

        public string Term { get; set; }

        public IDictionary<string, string> Indexes { get; } = new Dictionary<string, string>();

        #endregion Public Properties
    }

    public class SearchDocumentIndexQueryHandler : IQueryHandler<SearchDocumentIndexQuery, SearchDto> {

        #region Private Read-Only Fields

        private readonly IIndexProvider _indexProvider;
        private readonly IPublisherSubscriber _publisherSubscriber;

        #endregion Private Read-Only Fields

        #region Public Constructors

        public SearchDocumentIndexQueryHandler(IIndexProvider indexProvider, IPublisherSubscriber publisherSubscriber) {
            Prevent.ParameterNull(indexProvider, nameof(indexProvider));
            Prevent.ParameterNull(publisherSubscriber, nameof(publisherSubscriber));

            _indexProvider = indexProvider;
            _publisherSubscriber = publisherSubscriber;
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

        private SearchDto ExecuteSearch(IIndex index, ISearchBuilder searchBuilder, string label) {
            var searchIndexResult = new SearchDto {
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

        #region IQueryHandler<SearchDocumentIndexQuery, SearchDto> Members

        public Task<SearchDto> HandleAsync(SearchDocumentIndexQuery query, CancellationToken cancellationToken = default(CancellationToken)) {
            var actualStep = 0;
            var totalSteps = query.Indexes.Count;

            return Task.Run(() => {
                _publisherSubscriber.ProgressiveTaskStartAsync(
                    title: "",
                    actualStep: actualStep,
                    totalSteps: totalSteps
                );

                if (string.IsNullOrWhiteSpace(query.Term)) { return null; }

                var criterions = query.Term.Split(' ');
                var positiveTerms = criterions.Where(_ => !_.StartsWith("-")).ToArray();
                var negativeTerms = criterions.Except(positiveTerms).ToArray();

                var result = new List<SearchDto>();
                foreach (var index in GetIndexes(query.Indexes.Keys)) {
                    var searchBuilder = CreateSearchBuilder(index, positiveTerms, negativeTerms);
                    var searchIndexResult = ExecuteSearch(index, searchBuilder, query.Indexes[index.Name]);
                    result.Add(searchIndexResult);

                    if (cancellationToken.IsCancellationRequested) { break; }
                }
                return result;
            }, cancellationToken);
        }

        #endregion IQueryHandler<SearchDocumentIndexQuery, SearchDto> Members
    }
}