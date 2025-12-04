using N3O.Umbraco.Attributes;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Content;
using System.Collections.Generic;

namespace N3O.Umbraco.Cloud.Platforms.Content;

[UmbracoContent(PlatformsConstants.Platforms.Alias)]
public class PlatformsContent : UmbracoContent<PlatformsContent> {
    public IEnumerable<CampaignContent> Campaigns => Content().GetDescendantsOfCompositionTypeAs<CampaignContent>();
    public IEnumerable<ElementContent> Elements => Content().GetDescendantsOfCompositionTypeAs<ElementContent>();
}