using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Cloud.Platforms.Content;

[UmbracoContent(PlatformsConstants.Settings.Build.ThemeSettings)]
public class ThemeSettingsContent : UmbracoContent<ThemeSettingsContent> {
	public ThemeContent DefaultTheme => Content().Children.FirstOrDefault().As<ThemeContent>();
	public IEnumerable<ThemeContent> AdditionalThemes => Content().Children.As<ThemeContent>().Except(DefaultTheme);
	
}