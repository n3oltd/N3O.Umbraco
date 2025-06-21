using N3O.Umbraco.Cloud.Extensions;
using N3O.Umbraco.Cloud.Lookups;
using N3O.Umbraco.Cloud.Models;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Cloud;

public class OrganizationInfoAccessor : IOrganizationInfoAccessor {
    private readonly ICdnClient _cdnClient;

    public OrganizationInfoAccessor(ICdnClient cdnClient) {
        _cdnClient = cdnClient;
    }

    public async Task<IOrganizationInfo> GetOrganizationInfoAsync(CancellationToken cancellationToken = default) {
        var publishedOrganizationInfo = await _cdnClient.DownloadSubscriptionContentAsync<PublishedOrganizationInfo>(SubscriptionFiles.OrganizationInfo,
                                                                                                                     cancellationToken);

        return publishedOrganizationInfo;
    }
}