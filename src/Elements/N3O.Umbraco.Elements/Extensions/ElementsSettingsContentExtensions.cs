using N3O.Umbraco.Elements.Clients;
using N3O.Umbraco.Elements.Content;
using System.Collections.Generic;

namespace N3O.Umbraco.Elements.Extensions;

public static class ElementsSettingsContentExtensions {
    public static Dictionary<string, object> ToReq(this ElementsSettingsContent elementsSettings) {
        var colors = new Dictionary<string, object>();

        colors.Add("background", elementsSettings.Background);
        colors.Add("foreground", elementsSettings.Foreground);
        colors.Add("primary", elementsSettings.Primary);
        colors.Add("primary-foreground", elementsSettings.PrimaryForeground);
        colors.Add("secondary", elementsSettings.Secondary);
        colors.Add("secondary-foreground", elementsSettings.SecondaryForeground);
        colors.Add("accent", elementsSettings.Accent);
        colors.Add("accent-foreground", elementsSettings.AccentForeground);
        colors.Add("muted", elementsSettings.Muted);
        colors.Add("muted-foreground", elementsSettings.MutedForeground);
        colors.Add("card", elementsSettings.Card);
        colors.Add("card-foreground", elementsSettings.CardForeground);
        colors.Add("destructive", elementsSettings.Destructive);
        colors.Add("destructive-foreground", elementsSettings.DestructiveForeground);
        colors.Add("border", elementsSettings.Border);
        colors.Add("input", elementsSettings.Input);
        colors.Add("ring", elementsSettings.Ring);
        
        var theme = new Dictionary<string, object>();
        theme.Add("borderRadius", elementsSettings.BorderRadius);
        theme.Add("fontFamily", elementsSettings.FontFamily);
        theme.Add("headingFontFamily", elementsSettings.HeadingFontFamily);
        theme.Add("colors", colors);
        
        var elements = new Dictionary<string, object>();
        elements.Add("theme", theme);
        elements.Add("features", elementsSettings.Features);

        return elements;

    }
}