using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;

namespace N3O.Umbraco.Cloud.Platforms.Content;

[UmbracoContent(PlatformsConstants.Terminologies.Alias)]
public class TerminologiesContent : UmbracoContent<TerminologiesContent> {
    public string Campaigns => GetValue(x => x.Campaigns);
}