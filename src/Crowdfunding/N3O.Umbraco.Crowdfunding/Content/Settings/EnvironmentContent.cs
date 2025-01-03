using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding.Lookups;

namespace N3O.Umbraco.Crowdfunding.Content.Settings;

[UmbracoContent(CrowdfundingConstants.Settings.Environment.Alias)]
public class EnvironmentContent : UmbracoContent<EnvironmentContent> {
    public string Name => Content().Name;
    public string ApiKey => GetValue(x => x.ApiKey);
    public string Domain => GetValue(x => x.Domain);
    public bool Default => GetValue(x => x.Default);
    public CrowdfundingEnvironmentType Environment => GetValue(x => x.Environment);
}