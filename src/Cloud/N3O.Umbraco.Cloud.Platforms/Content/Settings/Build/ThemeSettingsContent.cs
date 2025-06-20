using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;

namespace N3O.Umbraco.Cloud.Platforms.Content;

[UmbracoContent(PlatformsConstants.Terminologies.Alias)]
public class ThemeSettingsContent : UmbracoContent<ThemeSettingsContent> {
	public string Accent  => GetValue(x => x.Accent);
	public string AccentForeground  => GetValue(x => x.AccentForeground);
	public string Background  => GetValue(x => x.Background);
	public string Border  => GetValue(x => x.Border);
	public int BorderRadius  => GetValue(x => x.BorderRadius);
	public string Card  => GetValue(x => x.Card);
	public string CardForeground  => GetValue(x => x.CardForeground);
	public string Destructive  => GetValue(x => x.Destructive);
	public string DestructiveForeground  => GetValue(x => x.DestructiveForeground);
	public string FontFamily  => GetValue(x => x.FontFamily);
	public string Foreground  => GetValue(x => x.Foreground);
	public string HeadingFontFamily  => GetValue(x => x.HeadingFontFamily);
	public string Input  => GetValue(x => x.Input);
	public string Muted  => GetValue(x => x.Muted);
	public string MutedForeground  => GetValue(x => x.MutedForeground);
	public string Primary  => GetValue(x => x.Primary);
	public string PrimaryForeground  => GetValue(x => x.PrimaryForeground);
	public string Ring  => GetValue(x => x.Ring);
	public string Secondary  => GetValue(x => x.Secondary);
	public string SecondaryForeground  => GetValue(x => x.SecondaryForeground);
}