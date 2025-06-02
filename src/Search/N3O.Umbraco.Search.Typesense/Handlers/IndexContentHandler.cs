using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Search.Typesense.Commands;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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
        var searchIndexer = _searchIndexers.FirstOrDefault(x => x.CanIndex(publishedContent));
        
        if (searchIndexer != null) {
            await searchIndexer.IndexAsync(publishedContent);
        }
        
        return None.Empty;
    }
}
