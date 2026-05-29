using Microsoft.Extensions.Diagnostics.HealthChecks;
using N3O.Umbraco.Attributes;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Hosting;

[HealthCheck("homepage", HealthCheckTags.Readiness)]
public class HomepageWarmupHealthCheck : IHealthCheck {
    private readonly HomepageWarmup _homepageWarmup;

    public HomepageWarmupHealthCheck(HomepageWarmup homepageWarmup) {
        _homepageWarmup = homepageWarmup;
    }

    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
                                                    CancellationToken cancellationToken = default) {
        if (_homepageWarmup.IsReady) {
            return Task.FromResult(HealthCheckResult.Healthy());
        }

        return Task.FromResult(HealthCheckResult.Unhealthy(_homepageWarmup.LastError ?? "warming up"));
    }
}
