using System.Collections.Generic;
using System.Linq;
using InfoFenix.Dto;
using InfoFenix.Models;
using InfoFenix.Search;

namespace InfoFenix.Services {

    public sealed class SearchService : ISearchService {

        #region Private Read-Only Fields

        private readonly IIndexProvider _indexProvider;

        #endregion Private Read-Only Fields

        #region Public Constructors

        public SearchService(IIndexProvider indexProvider) {
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
                        .WithField(Common.DocumentIndex.FieldNames.Content, item.ToLower(), item.Contains("*"))
                        .NotAnalyzed();
                } else {
                    searchBuilder = searchBuilder
                        .WithField(Common.DocumentIndex.FieldNames.Content, item.ToLower(), item.Contains("*"))
                        .ExactMatch()
                        .Mandatory();
                }
            }
            // Search negative terms
            foreach (var item in negativeTerms) {
                searchBuilder = searchBuilder
                    .WithField(Common.DocumentIndex.FieldNames.Content, item.ToLower(), false)
                    .Forbidden();
            }

            searchBuilder
                .SortByInteger(Common.DocumentIndex.FieldNames.DocumentCode)
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

        #endregion Private Methods

        #region Private Inner Classes

        private class SearchTerms {

            #region Public Properties

            public string[] PositiveTerms { get; set; } = Enumerable.Empty<string>().ToArray();

            public string[] NegativeTerms { get; set; } = Enumerable.Empty<string>().ToArray();

            #endregion Public Properties

            #region Public Static Methods

            public static SearchTerms Parse(string[] terms) {
                var positiveTerms = terms.Where(_ => !_.StartsWith("-")).ToArray();
                var negativeTerms = terms.Except(positiveTerms).ToArray();

                return new SearchTerms {
                    PositiveTerms = positiveTerms,
                    NegativeTerms = negativeTerms
                };
            }

            #endregion Public Static Methods
        }

        #endregion Private Inner Classes

        #region ISearchService Members

        public IEnumerable<SearchResult> ExecuteSearch(string[] terms, params string[] indexes) {
            var searchTerms = SearchTerms.Parse(terms);
            foreach (var index in GetIndexes(indexes)) {
                var searchBuilder = CreateSearchBuilder(index, searchTerms.PositiveTerms, searchTerms.NegativeTerms);
                yield return new SearchResult {
                    Terms = searchTerms.PositiveTerms,
                    Documents = searchBuilder.Search().Select(DocumentIndexDto.Map).ToArray()
                };
            }
        }

        public IEnumerable<SearchStatistic> GetStatistics(string[] terms, params string[] indexes) {
            var searchTerms = SearchTerms.Parse(terms);
            foreach (var index in GetIndexes(indexes)) {
                var searchBuilder = CreateSearchBuilder(index, searchTerms.PositiveTerms, searchTerms.NegativeTerms);
                yield return new SearchStatistic {
                    IndexName = index.Name,
                    TotalDocuments = index.TotalDocuments(),
                    TotalDocumentsFound = searchBuilder.Count()
                };
            }
        }

        #endregion ISearchService Members
    }
}