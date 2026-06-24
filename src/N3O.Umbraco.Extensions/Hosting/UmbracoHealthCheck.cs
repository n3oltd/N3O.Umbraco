using Microsoft.Extensions.Diagnostics.HealthChecks;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Extensions;
using System;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Services.Navigation;

namespace N3O.Umbraco.Hosting;

[HealthCheck("umbraco", HealthCheckTags.Readiness)]
public class UmbracoHealthCheck : IHealthCheck {
    private readonly IDocumentNavigationQueryService _navigationQueryService;
    private readonly IPublishedContentCache _publishedContentCache;

    public UmbracoHealthCheck(IDocumentNavigationQueryService navigationQueryService,
                              IPublishedContentCache publishedContentCache) {
        _navigationQueryService = navigationQueryService;
        _publishedContentCache = publishedContentCache;
    }

    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
                                                    CancellationToken cancellationToken = default) {
        try {
            // Touch the content tree to confirm the published cache is initialised and the
            // underlying database is reachable. A brand-new site with no published content
            // will return an empty list which is still Healthy; only an exception (cache
            // not built, DB unreachable, Umbraco still starting) flips us to Unhealthy.
            
            // TODO not necessarily accurate, if cache is not found for a content, in newer versions the content is fetched from the
            // database and converted to a PublishedContent and returned which is not correct for this purpose
            _ = _navigationQueryService.GetPublishedRootContents(_publishedContentCache);;

            return Task.FromResult(HealthCheckResult.Healthy());
        } catch (Exception ex) {
            return Task.FromResult(HealthCheckResult.Unhealthy(exception: ex));
        }
    }
}
