using Microsoft.Extensions.Diagnostics.HealthChecks;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Services.Navigation;
using Umbraco.Cms.Core.Web;

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
            _navigationQueryService.TryGetRootKeys(out var rootKeys);
            
            _ = rootKeys.Select(_publishedContentCache.GetById).ToList();

            return Task.FromResult(HealthCheckResult.Healthy());
        } catch (Exception ex) {
            return Task.FromResult(HealthCheckResult.Unhealthy(exception: ex));
        }
    }
}
