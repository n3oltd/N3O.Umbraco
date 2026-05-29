using Microsoft.Extensions.Diagnostics.HealthChecks;
using N3O.Umbraco.Attributes;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Web;

namespace N3O.Umbraco.Hosting;

[HealthCheck("umbraco", HealthCheckTags.Readiness)]
public class UmbracoHealthCheck : IHealthCheck {
    private readonly IUmbracoContextFactory _umbracoContextFactory;

    public UmbracoHealthCheck(IUmbracoContextFactory umbracoContextFactory) {
        _umbracoContextFactory = umbracoContextFactory;
    }

    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
                                                    CancellationToken cancellationToken = default) {
        try {
            using var reference = _umbracoContextFactory.EnsureUmbracoContext();

            // Touch the content tree to confirm the published cache is initialised and the
            // underlying database is reachable. A brand-new site with no published content
            // will return an empty list which is still Healthy; only an exception (cache
            // not built, DB unreachable, Umbraco still starting) flips us to Unhealthy.
            _ = reference.UmbracoContext.Content.GetAtRoot().ToList();

            return Task.FromResult(HealthCheckResult.Healthy());
        } catch (Exception ex) {
            return Task.FromResult(HealthCheckResult.Unhealthy(exception: ex));
        }
    }
}
