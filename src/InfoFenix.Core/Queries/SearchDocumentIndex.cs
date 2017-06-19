using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using InfoFenix.Core.Cqrs;
using InfoFenix.Core.Dto;
using InfoFenix.Core.PubSub;
using InfoFenix.Core.Search;
using Resource = InfoFenix.Core.Resources.Resources;

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

        private IEnumerable<DocumentIndexDto> ExecuteSearch(IIndex index, ISearchBuilder searchBuilder, string label) {
            foreach (var searchHit in searchBuilder.Search()) {
                yield return DocumentIndexDto.Map(searchHit);
            }
        }

        #endregion Private Methods

        #region IQueryHandler<SearchDocumentIndexQuery, SearchDto> Members

        public Task<SearchDto> HandleAsync(SearchDocumentIndexQuery query, CancellationToken cancellationToken = default(CancellationToken)) {
            var actualStep = 0;
            var totalSteps = query.Indexes.Count + 1;

            return Task.Run(() => {
                _publisherSubscriber.ProgressiveTaskStartAsync(
                    title: Resource.SearchDocumentIndex_ProgressiveTaskStart_Title,
                    actualStep: actualStep,
                    totalSteps: totalSteps
                );

                _publisherSubscriber.ProgressiveTaskPerformStepAsync(
                    message: Resource.SearchDocumentIndex_ProgressiveTaskPerformStep_Build_Message,
                    actualStep: ++actualStep,
                    totalSteps: totalSteps
                );

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
                    _publisherSubscriber.ProgressiveTaskCompleteAsync(
                        actualStep: actualStep,
                        totalSteps: totalSteps
                    );

                    return searchDto;
                }

                var criterions = query.QueryTerm.Split(' ');
                var positiveTerms = criterions.Where(_ => !_.StartsWith("-")).ToArray();
                var negativeTerms = criterions.Except(positiveTerms).ToArray();
                foreach (var index in indexes) {
                    if (cancellationToken.IsCancellationRequested) { break; }

                    var indexDto = searchDto[index.Name];

                    _publisherSubscriber.ProgressiveTaskPerformStepAsync(
                        message: string.Format(Resource.SearchDocumentIndex_ProgressiveTaskPerformStep_Search_Message, indexDto.Label),
                        actualStep: ++actualStep,
                        totalSteps: totalSteps
                    );

                    var searchBuilder = CreateSearchBuilder(index, positiveTerms, negativeTerms);
                    var documents = ExecuteSearch(index, searchBuilder, indexDto.Label);

                    indexDto.Documents = documents.ToList();
                }

                if (cancellationToken.IsCancellationRequested) {
                    _publisherSubscriber.ProgressiveTaskCancelAsync(
                        actualStep: actualStep,
                        totalSteps: totalSteps
                    );
                }

                _publisherSubscriber.ProgressiveTaskCompleteAsync(
                    actualStep: actualStep,
                    totalSteps: totalSteps
                );

                return searchDto;
            }, cancellationToken);
        }

        #endregion IQueryHandler<SearchDocumentIndexQuery, SearchDto> Members
    }
}