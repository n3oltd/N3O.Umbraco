using N3O.Umbraco.Content;

namespace N3O.Umbraco.Analytics.Content;

public class  AttributionContent : UmbracoContent<AttributionContent> {
    public int UtmSource => GetValue(x => x.UtmSource);
    public int UtmMedium => GetValue(x => x.UtmMedium);
    public int UtmCampaign => GetValue(x => x.UtmCampaign);
}