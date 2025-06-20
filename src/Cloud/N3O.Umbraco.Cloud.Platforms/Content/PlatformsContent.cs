using N3O.Umbraco.Cloud.Platforms.Content.Settings;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Content;
using System.Collections.Generic;

namespace N3O.Umbraco.Cloud.Platforms.Content;

public class PlatformsContent : UmbracoContent<PlatformsContent> {
    public PlatformsSettingsContent Settings => Content().GetSingleChildOfTypeAs<PlatformsSettingsContent>();
    public IEnumerable<CampaignContent> Campaigns => Content().GetDescendantsOfCompositionTypeAs<CampaignContent>();
    public IEnumerable<ElementContent> Elements => Content().GetDescendantsOfCompositionTypeAs<ElementContent>();
}