using N3O.Umbraco.Entities;
using N3O.Umbraco.Extensions;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Cloud.Platforms;

public class CampaignIdAccessor : ICampaignIdAccessor {
    private readonly IReadOnlyList<ICampaignIdProvider> _campaignIdProviders;

    public CampaignIdAccessor(IEnumerable<ICampaignIdProvider> campaignIdProviders) {
        _campaignIdProviders = campaignIdProviders.ApplyAttributeOrdering();
    }
    
    public async Task<EntityId> GetIdAsync(IPublishedContent content, CancellationToken cancellationToken = default) {
        foreach (var campaignIdProvider in _campaignIdProviders) {
            var campaignId = await campaignIdProvider.GetIdAsync(content, cancellationToken);

            if (campaignId.HasValue()) {
                return campaignId;
            }
        }
        
        return null;
    }
}
