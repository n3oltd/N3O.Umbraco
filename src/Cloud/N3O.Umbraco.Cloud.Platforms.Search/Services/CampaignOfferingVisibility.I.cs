using N3O.Umbraco.Cloud.Platforms.Clients;

namespace N3O.Umbraco.Cloud.Platforms.Search;

public interface ICampaignOfferingVisibility {
    bool IsVisible(PublishedCampaign campaign);
    bool IsVisible(PublishedOffering offering);
}
