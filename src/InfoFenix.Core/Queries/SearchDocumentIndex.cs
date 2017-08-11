using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InfoFenix.Core.CQRS;
using InfoFenix.Core.Entities;
using InfoFenix.Core.Search;

namespace InfoFenix.Core.Queries {

    public class SearchDocumentIndexQuery : IQuery<SearchDto> {

        #region Public Properties

        public string QueryTerm { get; set; }

        public IDictionary<string, string> Indexes { get; set; } = new Dictionary<string, string>();

        #endregion Public Properties
    }

    public class SearchDocumentIndexQueryHandler : IQueryHandler<SearchDocumentIndexQuery, SearchDto> {

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

        private IEnumerable<DocumentIndexDto> ExecuteSearch(IIndex index, ISearchBuilder searchBuilder, string label) {
            return searchBuilder.Search().Select(DocumentIndexDto.Map);
        }

        #endregion Private Methods

        #region IQueryHandler<SearchDocumentIndexQuery, SearchDto> Members

        public Task<SearchDto> HandleAsync(SearchDocumentIndexQuery query, CancellationToken cancellationToken = default(CancellationToken)) {
            return Task.Run(() => {
                var indexes = GetIndexes(query.Indexes.Keys).ToArray();
                var searchDto = new SearchDto {
                    QueryTerm = query.QueryTerm,
                    Indexes = indexes.Select(_ => new IndexDto {
                        Label = query.Indexes[_.Name],
                        Name = _.Name,
                        TotalDocuments = _.TotalDocuments()
                    }).ToList()
                };

                if (string.IsNullOrWhiteSpace(query.QueryTerm)) {
                    return searchDto;
                }

                var criterions = query.QueryTerm.Split(' ');
                var positiveTerms = criterions.Where(_ => !_.StartsWith("-")).ToArray();
                var negativeTerms = criterions.Except(positiveTerms).ToArray();
                foreach (var index in indexes) {
                    if (cancellationToken.IsCancellationRequested) { break; }

                    var indexDto = searchDto[index.Name];
                    var searchBuilder = CreateSearchBuilder(index, positiveTerms, negativeTerms);
                    var documents = ExecuteSearch(index, searchBuilder, indexDto.Label);

                    indexDto.DocumentsFound = documents.ToList();
                }

                return searchDto;
            }, cancellationToken);
        }

        #endregion IQueryHandler<SearchDocumentIndexQuery, SearchDto> Members
    }
}