using N3O.Umbraco.Content;

namespace N3O.Umbraco.Analytics.Content;

public class  AttributionSettingsContent : UmbracoContent<AttributionSettingsContent> {
    public int? UtmCampaignDimensionIndex => GetValue(x => x.UtmCampaignDimensionIndex);
    public int? UtmMediumDimensionIndex => GetValue(x => x.UtmMediumDimensionIndex);
    public int? UtmSourceDimensionIndex => GetValue(x => x.UtmSourceDimensionIndex);
}