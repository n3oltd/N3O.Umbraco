using System.Threading;
using System.Threading.Tasks;
using Typesense;

namespace N3O.Umbraco.Search.Typesense;

public class Searcher<TDocument> : ISearcher<TDocument> where TDocument : class {
    private readonly ITypesenseClient _typesenseClient;

    public Searcher(ITypesenseClient typesenseClient) {
        _typesenseClient = typesenseClient;
    }

    public async Task<SearchResult<TDocument>> SearchAsync(SearchParameters searchParameters,
                                                           CancellationToken cancellationToken = default) {
        var collection = TypesenseHelper.GetCollectionName<TDocument>();

        var results = await _typesenseClient.Search<TDocument>(collection, searchParameters, cancellationToken);

        return results;
    }
}