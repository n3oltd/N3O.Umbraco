using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;

namespace N3O.Umbraco.Crowdfunding.Content.Settings;

[UmbracoContent(CrowdfundingConstants.Settings.Alias)]
public class SettingsContent : UmbracoContent<SettingsContent> {
    public string ApiKey => GetValue(x => x.ApiKey);
}