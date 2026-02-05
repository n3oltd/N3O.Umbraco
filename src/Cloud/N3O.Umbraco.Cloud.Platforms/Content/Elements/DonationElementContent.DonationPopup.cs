using N3O.Umbraco.Attributes;

namespace N3O.Umbraco.Cloud.Platforms.Content;

[UmbracoContent(PlatformsConstants.Elements.DonationPopup)]
public class DonationPopupElementContent : DonationElementContent<DonationPopupElementContent> {
    public int? TimeDelaySeconds => GetValue(x => x.TimeDelaySeconds);
}