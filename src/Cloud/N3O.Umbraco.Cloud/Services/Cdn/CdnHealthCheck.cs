using Microsoft.Extensions.Diagnostics.HealthChecks;
using N3O.Umbraco.Attributes;
using N3O.Umbraco.Cloud.Extensions;
using N3O.Umbraco.Cloud.Lookups;
using N3O.Umbraco.Cloud.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Cloud;

[HealthCheck("cdn")]
public class CdnHealthCheck : IHealthCheck {
    private readonly ICdnClient _cdnClient;

    public CdnHealthCheck(ICdnClient cdnClient) {
        _cdnClient = cdnClient;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
                                                          CancellationToken cancellationToken = default) {
        try {
            var content = await _cdnClient.DownloadSubscriptionContentAsync<PublishedSubscriptionOrganization>(SubscriptionFiles.OrganizationInfo,
                                                                                                               JsonSerializers.JsonProvider,
                                                                                                               cancellationToken);

            if (content == null) {
                return HealthCheckResult.Unhealthy($"CDN returned null for {SubscriptionFiles.OrganizationInfo.Filename}");
            }

            return HealthCheckResult.Healthy();
        } catch (Exception ex) {
            return HealthCheckResult.Unhealthy(exception: ex);
        }
    }
}
