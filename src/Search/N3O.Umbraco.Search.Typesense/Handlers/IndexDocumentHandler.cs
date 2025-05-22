using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Mediator;
using N3O.Umbraco.Search.Typesense.Commands;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using N3O.Umbraco.Search.Typesense.Indexing;
using N3O.Umbraco.Search.Typesense.Services;

namespace N3O.Umbraco.Search.Typesense.Handlers;

public class IndexDocumentHandler : IRequestHandler<IndexDocumentCommand, None, None> {
    private readonly IContentLocator _contentLocator;
    private readonly ISearchDocumentBuilder _searchDocumentBuilder;
    private readonly ITypesenseService _typesenseService;
    private readonly IReadOnlyList<ISearchIndexer> _searchIndexers;
    private readonly ILogger<IndexDocumentHandler> _logger;

    public IndexDocumentHandler(IContentLocator contentLocator,
                                ISearchDocumentBuilder searchDocumentBuilder,
                                ITypesenseService typesenseService,
                                IEnumerable<ISearchIndexer> searchIndexers, ILogger<IndexDocumentHandler> logger) {
        _contentLocator = contentLocator;
        _searchDocumentBuilder = searchDocumentBuilder;
        _typesenseService = typesenseService;
        _searchIndexers = searchIndexers.ApplyAttributeOrdering();
        _logger = logger;
    }
    
    public async Task<None> Handle(IndexDocumentCommand req, CancellationToken cancellationToken) {
        var publishedContent = req.ContentId.Run(id => _contentLocator.ById(id), true);
        var searchIndexer = _searchIndexers.FirstOrDefault(x => x.CanIndex(publishedContent));
        
        await _typesenseService.CheckConnectionAsync(cancellationToken);
        if (searchIndexer.HasValue()) {
            searchIndexer.Index(_searchDocumentBuilder, publishedContent);
            var searchDocument = _searchDocumentBuilder.Build();

            await _typesenseService.UpsertAsync(searchDocument,cancellationToken);
        }
        
        return None.Empty;
    }
}
