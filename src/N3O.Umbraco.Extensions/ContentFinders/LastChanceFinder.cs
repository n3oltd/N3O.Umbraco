using Microsoft.Extensions.Logging;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Lookups;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Routing;

namespace N3O.Umbraco.ContentFinders;

public class LastChanceFinder : IContentLastChanceFinder {
    private readonly IContentCache _contentCache;
    private readonly ILogger<LastChanceFinder> _logger;

    public LastChanceFinder(IContentCache contentCache, ILogger<LastChanceFinder> logger) {
        _contentCache = contentCache;
        _logger = logger;
    }

    public Task<bool> TryFindContent(IPublishedRequestBuilder request) {
        if (request != null && request.ResponseStatusCode == 404) {
            _logger.LogError("No page found for path {Path}", request.AbsolutePathDecoded);
            
            request.SetIs404();

            var notFoundPage = _contentCache.Special(SpecialPages.NotFound);
            
            _logger.LogError("Not found page with id {ID} found", notFoundPage.Key);
            
            request.SetPublishedContent(notFoundPage);
            
            return Task.FromResult(true);
        }

        return Task.FromResult(false);
    }
}
