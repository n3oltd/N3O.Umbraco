using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Search.Typesense.Commands;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Extensions;

namespace N3O.Umbraco.Search.Typesense.Handlers;

public class IndexContentHandler : IRequestHandler<IndexContentCommand, None, None> {
    private readonly IContentLocator _contentLocator;
    private readonly IReadOnlyList<ISearchIndexer> _searchIndexers;

    public IndexContentHandler(IContentLocator contentLocator, IEnumerable<ISearchIndexer> searchIndexers) {
        _contentLocator = contentLocator;
        _searchIndexers = searchIndexers.ApplyAttributeOrdering();
    }
    
    public async Task<None> Handle(IndexContentCommand req, CancellationToken cancellationToken) {
        var publishedContent = req.ContentId.Run(id => _contentLocator.ById(id), true);
        var searchIndexers = _searchIndexers.Where(x => x.CanIndex(publishedContent)).OrEmpty().ToList();
        
        if (searchIndexers.HasAny()) {
            foreach (var searchIndexer in searchIndexers) {
                if (publishedContent.ContentType.VariesByCulture()) {
                    // Remove all existing culture documents first so cultures that are no longer
                    // published (and so won't be re-indexed below) don't linger in the index
                    await searchIndexer.DeleteAsync(publishedContent.Key);

                    foreach (var publishedContentCulture in publishedContent.Cultures) {
                        await searchIndexer.IndexAsync(publishedContent, publishedContentCulture.Value.Culture);
                    }
                } else {
                    await searchIndexer.IndexAsync(publishedContent);
                }
            }
        }
        
        return None.Empty;
    }
}
