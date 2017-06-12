using System.Collections.Generic;
using System.Linq;
using InfoFenix.Core.Cqrs;
using InfoFenix.Core.Queries;
using InfoFenix.Core.Search;

namespace InfoFenix.Core.Services {

    public class SearchIndexService : ISearchIndexService {

        #region Private Read-Only Fields

        private readonly ICommandQueryDispatcher _commandQueryDispatcher;
        private readonly IIndexProvider _indexProvider;

        #endregion Private Read-Only Fields

        #region Public Constructors

        public SearchIndexService(ICommandQueryDispatcher commandQueryDispatcher, IIndexProvider indexProvider) {
            Prevent.ParameterNull(commandQueryDispatcher, nameof(commandQueryDispatcher));
            Prevent.ParameterNull(indexProvider, nameof(indexProvider));

            _commandQueryDispatcher = commandQueryDispatcher;
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

        private IEnumerable<IIndex> GetIndexes(string[] indexNames) {
            if (indexNames.IsNullOrEmpty()) { indexNames = _indexProvider.List().ToArray(); }
            foreach (var indexName in indexNames) {
                yield return _indexProvider.GetOrCreate(indexName);
            }
        }

        private SearchIndexCollection ExecuteSearch(IIndex index, ISearchBuilder searchBuilder) {
            // Map search hits
            var searchIndexCollection = new SearchIndexCollection {
                IndexName = index.Name,
                TotalDocuments = index.TotalDocuments()
            };
            foreach (var searchHit in searchBuilder.Search()) {
                var document = _commandQueryDispatcher.Query(new GetDocumentQuery { ID = int.Parse(searchHit.DocumentID) });
                dynamic searchIndexItem = new SearchIndexItem();
                searchIndexItem.ID = document.ID;
                searchIndexItem.Path = document.Path;
                searchIndexItem.Code = document.Code;
                searchIndexItem.Payload = document.Payload;
                searchIndexCollection.Add(searchIndexItem);
            }
            return searchIndexCollection;
        }

        #endregion Private Methods

        #region ISearchIndexService Members

        public SearchIndexResultSet AvaliableIndexes() {
            var result = new SearchIndexResultSet();
            foreach (var index in GetIndexes(null)) {
                result.Add(new SearchIndexCollection {
                    IndexName = index.Name,
                    TotalDocuments = index.TotalDocuments()
                });
            }
            return result;
        }

        public SearchIndexResultSet Search(string term, params string[] indexNames) {
            if (string.IsNullOrWhiteSpace(term)) { return SearchIndexResultSet.Empty; }

            var criterions = term.Split(' ');
            var positiveTerms = criterions.Where(_ => !_.StartsWith("-")).ToArray();
            var negativeTerms = criterions.Except(positiveTerms).ToArray();

            var result = new SearchIndexResultSet();
            foreach (var index in GetIndexes(indexNames)) {
                var searchBuilder = CreateSearchBuilder(index, positiveTerms, negativeTerms);
                var searchIndexCollection = ExecuteSearch(index, searchBuilder);
                result.Add(searchIndexCollection);
            }
            return result;
        }

        #endregion ISearchIndexService Members
    }
}