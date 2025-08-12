using N3O.Umbraco.Extensions;
using N3O.Umbraco.Json;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Typesense;

namespace N3O.Umbraco.Search.Typesense;

public class Searcher<TDocument> : ISearcher<TDocument> where TDocument : class {
    private readonly ITypesenseClient _typesenseClient;
    private readonly IJsonProvider _jsonProvider;

    public Searcher(ITypesenseClient typesenseClient, IJsonProvider jsonProvider) {
        _typesenseClient = typesenseClient;
        _jsonProvider = jsonProvider;
    }

    public async Task<SearchResult<TDocument>> SearchAsync(SearchParameters searchParameters,
                                                           CancellationToken cancellationToken = default) {
        var collection = TypesenseHelper.GetCollectionName<TDocument>();

        var results = await _typesenseClient.Search<object>(collection, searchParameters, cancellationToken);

        return ToTypedResults(results);
    }

    private SearchResult<TDocument> ToTypedResults(SearchResult<object> results) {
        var typedHits = results.Hits.Select(ToTypedHit).ToList();
        
        return new SearchResult<TDocument>(results.FacetCounts,
                                           results.Found,
                                           results.OutOf,
                                           results.Page,
                                           results.SearchTimeMs,
                                           results.TookMs,
                                           typedHits);
    }

    private Hit<TDocument> ToTypedHit(Hit<object> hit) {
        var typedHit = hit.Document.IfNotNull(x => _jsonProvider.DeserializeObject<TDocument>(x.ToString()));
        
        return new Hit<TDocument>(hit.Highlights, typedHit, hit.TextMatch, hit.VectorDistance, hit.GeoDistanceMeters);
    }
}