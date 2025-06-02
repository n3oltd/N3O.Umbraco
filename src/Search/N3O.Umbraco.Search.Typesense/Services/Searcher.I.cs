using System.Threading;
using System.Threading.Tasks;
using Typesense;

namespace N3O.Umbraco.Search.Typesense;

public interface ISearcher<TDocument> where TDocument : class {
    Task<SearchResult<TDocument>> SearchAsync(SearchParameters searchParameters,
                                              CancellationToken cancellationToken = default);
}