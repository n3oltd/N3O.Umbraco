﻿using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;

namespace N3O.Umbraco.Cloud.Platforms.Content;

[UmbracoContent(PlatformsConstants.Settings.Build.Theme)]
public class ThemeSettingsContent : UmbracoContent<ThemeSettingsContent> {
	public string Accent  => GetValue(x => x.Accent);
	public string AccentForeground  => GetValue(x => x.AccentForeground);
	public string Background  => GetValue(x => x.Background);
	public string Border  => GetValue(x => x.Border);
	public int BorderRadius  => GetValue(x => x.BorderRadius);
	public string Card  => GetValue(x => x.Card);
	public string CardForeground  => GetValue(x => x.CardForeground);
	public string Chart1 => GetValue(x => x.Chart1);
	public string Chart2 => GetValue(x => x.Chart2);
	public string Chart3 => GetValue(x => x.Chart3);
	public string Chart4 => GetValue(x => x.Chart4);
	public string Chart5 => GetValue(x => x.Chart5);
	public string Destructive  => GetValue(x => x.Destructive);
	public string DestructiveForeground  => GetValue(x => x.DestructiveForeground);
	public string FontFamily  => GetValue(x => x.FontFamily);
	public string Foreground  => GetValue(x => x.Foreground);
	public string HeadingFontFamily  => GetValue(x => x.HeadingFontFamily);
	public string Input  => GetValue(x => x.Input);
	public string Muted  => GetValue(x => x.Muted);
	public string MutedForeground  => GetValue(x => x.MutedForeground);
	public string Popover  => GetValue(x => x.Popover);
	public string PopoverForeground  => GetValue(x => x.PopoverForeground);
	public string Primary  => GetValue(x => x.Primary);
	public string PrimaryForeground  => GetValue(x => x.PrimaryForeground);
	public string Ring  => GetValue(x => x.Ring);
	public string Secondary  => GetValue(x => x.Secondary);
	public string SecondaryForeground  => GetValue(x => x.SecondaryForeground);
}