using N3O.Umbraco.Attributes;
using N3O.Umbraco.Cloud.Platforms.Extensions;
using N3O.Umbraco.Content;

namespace N3O.Umbraco.Cloud.Platforms.Content;

[UmbracoContent(PlatformsConstants.Settings.Build.Alias)]
public class BuildSettingsContent : UmbracoContent<BuildSettingsContent> {
    public ThemeSettingsContent Theme => Content().GetSingleChildOfTypeAs<ThemeSettingsContent>();
}