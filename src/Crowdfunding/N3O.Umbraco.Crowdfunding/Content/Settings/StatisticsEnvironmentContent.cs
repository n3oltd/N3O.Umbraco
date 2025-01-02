using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using N3O.Umbraco.Crowdfunding.Lookups;

namespace N3O.Umbraco.Crowdfunding.Content.Settings;

[UmbracoContent(CrowdfundingConstants.Settings.StatisticsEnvironment.Alias)]
public class StatisticsEnvironmentContent : UmbracoContent<StatisticsEnvironmentContent> {
    public string Name => Content().Name;
    public string ApiKey => GetValue(x => x.ApiKey);
    public string Domain => GetValue(x => x.Domain);
    public bool Default => GetValue(x => x.Default);
    public StatisticsEnvironmentType StatisticsEnvironment => GetValue(x => x.StatisticsEnvironment);
}