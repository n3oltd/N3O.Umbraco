using Microsoft.Extensions.Logging;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using System;
using System.Linq;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Routing;
using Umbraco.Extensions;

namespace N3O.Umbraco.ContentFinders;

public abstract class ContentFinder : IContentFinder {
    private readonly ILogger<ContentFinder> _logger;

    protected ContentFinder(ILogger<ContentFinder> logger, IContentCache contentCache) {
        _logger = logger;
        ContentCache = contentCache;
    }

    public async Task<bool> TryFindContent(IPublishedRequestBuilder request) {
        try {
            return await FindContentAsync(request);
        } catch (Exception ex) {
            _logger.LogError(ex, "Error executing content finder");
            
            return false;
        }
    }

    protected abstract Task<bool> FindContentAsync(IPublishedRequestBuilder request);

    protected Task<bool> TryFindRelocatedContentAsync(string pageTypeAlias,
                                                      string contentTypeAlias,
                                                      string contentCollectionTypeAlias,
                                                      IPublishedRequestBuilder request) {
        var page = ContentCache.Single(pageTypeAlias);
        var contentCollection = ContentCache.Single(contentCollectionTypeAlias);

        if (page == null || contentCollection == null) {
            return Task.FromResult(false);
        }
    
        var pagePath = page.RelativeUrl();
    
        var path = GetRequestedPath(request.Uri, pagePath);

        if (path == null) {
            return Task.FromResult(false);
        }

        var match = contentCollection.Descendants()
                                     .Where(x => x.ContentType.Alias.EqualsInvariant(contentTypeAlias))
                                     .FirstOrDefault(x => StrippedPath(x, pagePath).EqualsInvariant(path));

        if (match != null) {
            request.SetPublishedContent(match);

            return Task.FromResult(true);
        }

        return Task.FromResult(false);
    }

    protected string StrippedPath(IPublishedContent content, string strip) {
        return content.RelativeUrl().Substring(strip.Length).RemoveLeadingSlashes();
    }

    protected string GetRequestedPath(Uri url, string strip = null) {
        var path = url.GetAbsolutePathDecoded().ToLowerInvariant();
        
        strip ??= "";

        if (strip.HasValue() && !path.StartsWith(strip.ToLowerInvariant())) {
            return null;
        }
    
        return path.Substring(strip.Length).RemoveLeadingSlashes();
    }

    protected IContentCache ContentCache { get; }
}
