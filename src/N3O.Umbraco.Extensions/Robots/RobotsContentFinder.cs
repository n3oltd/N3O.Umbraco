using Microsoft.Extensions.Logging;
using N3O.Umbraco.Content;
using N3O.Umbraco.ContentFinders;
using N3O.Umbraco.Extensions;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Routing;

namespace N3O.Umbraco.Robots;

public class RobotsContentFinder : ContentFinder {
    private static readonly string RobotsAlias = AliasHelper<RobotsContent>.ContentTypeAlias();

    public RobotsContentFinder(ILogger<RobotsContentFinder> logger, IContentCache contentCache)
        : base(logger, contentCache) { }

    protected override Task<bool> FindContentAsync(IPublishedRequestBuilder request) {
        var path = GetRequestedPath(request.Uri);

        if (path.EqualsInvariant(RobotsTxt.File)) {
            request.SetPublishedContent(ContentCache.Single(RobotsAlias));
            
            return Task.FromResult(true);
        }

        return Task.FromResult(false);
    }
}
