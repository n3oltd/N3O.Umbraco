using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;

namespace N3O.Umbraco.Crowdfunding.Content;

[UmbracoContent(CrowdfundingConstants.Settings.TemplateSettings.Alias)]
public class TemplateSettingsContent : UmbracoContent<TemplateSettingsContent> {
    public string CssVariables => GetValue(x => x.CssVariables);
}