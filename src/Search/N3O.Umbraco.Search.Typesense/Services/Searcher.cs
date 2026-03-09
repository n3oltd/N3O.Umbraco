using N3O.Umbraco.Extensions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Typesense;

namespace N3O.Umbraco.Search.Typesense;

public class Searcher<TDocument> : ISearcher<TDocument> where TDocument : class {
    private readonly ITypesenseClient _typesenseClient;
    private readonly ITypesenseJsonProvider _typesenseJsonProvider;

    public Searcher(ITypesenseClient typesenseClient, ITypesenseJsonProvider typesenseJsonProvider) {
        _typesenseClient = typesenseClient;
        _typesenseJsonProvider = typesenseJsonProvider;
    }

    public async Task<SearchResult<TDocument>> SearchAsync(SearchParameters searchParameters,
                                                           CancellationToken cancellationToken = default) {
        var collectionInfo = TypesenseHelper.GetCollection<TDocument>();

        var results = await _typesenseClient.Search<object>(collectionInfo.Name.Resolve(), searchParameters, cancellationToken);

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
        var typedHit = hit.Document.IfNotNull(x => _typesenseJsonProvider.DeserializeObject<TDocument>(x.ToString()));
        
        return new Hit<TDocument>(hit.Highlights, typedHit, hit.TextMatch, hit.VectorDistance, hit.GeoDistanceMeters);
    }
}