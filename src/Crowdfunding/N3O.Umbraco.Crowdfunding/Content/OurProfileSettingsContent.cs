using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using N3O.Umbraco.Uploader.Models;

namespace N3O.Umbraco.Crowdfunding.Content;

[UmbracoContent(CrowdfundingConstants.OurProfileSettings.Alias)]
public class OurProfileSettingsContent : UmbracoElement<OurProfileSettingsContent> {
    public FileUpload ProfileImage => GetValue(x => x.ProfileImage);
    public string DisplayName => GetValue(x => x.DisplayName);
    public string Strapline => GetValue(x => x.Strapline);
}