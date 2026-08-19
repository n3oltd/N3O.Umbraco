using N3O.Umbraco.Cloud.Platforms.Clients;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Cloud.Platforms.Search;

public class CampaignOfferingVisibility : ICampaignOfferingVisibility {
    private readonly IReadOnlyList<ICampaignOfferingVisibilityFilter> _filters;

    public CampaignOfferingVisibility(IEnumerable<ICampaignOfferingVisibilityFilter> filters) {
        _filters = filters.ToList();
    }

    public bool IsVisible(PublishedCampaign campaign) {
        return _filters.All(x => x.IsVisible(campaign));
    }

    public bool IsVisible(PublishedOffering offering) {
        return _filters.All(x => x.IsVisible(offering));
    }
}
