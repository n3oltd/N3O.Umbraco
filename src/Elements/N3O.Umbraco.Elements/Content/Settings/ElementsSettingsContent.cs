using N3O.Umbraco.Content;
using System.Collections.Generic;

namespace N3O.Umbraco.Elements.Content;

public class ElementsSettingsContent : UmbracoContent<ElementsSettingsContent> {
    public int BorderRadius => GetValue(x => x.BorderRadius);
    public string FontFamily => GetValue(x => x.FontFamily);
    public string HeadingFontFamily => GetValue(x => x.HeadingFontFamily);
    
    public string Background => GetValue(x => x.Background);
    public string Foreground => GetValue(x => x.Foreground);
    public string Primary => GetValue(x => x.Primary);
    public string PrimaryForeground => GetValue(x => x.PrimaryForeground);
    public string Secondary => GetValue(x => x.Secondary);
    public string SecondaryForeground => GetValue(x => x.SecondaryForeground);
    public string Accent => GetValue(x => x.Accent);
    public string AccentForeground => GetValue(x => x.AccentForeground);
    public string Muted => GetValue(x => x.Muted);
    public string MutedForeground => GetValue(x => x.MutedForeground);
    public string Card => GetValue(x => x.Card);
    public string CardForeground => GetValue(x => x.CardForeground);
    public string Destructive => GetValue(x => x.Destructive);
    public string DestructiveForeground => GetValue(x => x.DestructiveForeground);
    public string Border => GetValue(x => x.Border);
    public string Input => GetValue(x => x.Input);
    public string Ring => GetValue(x => x.Ring);
    
    public IEnumerable<string> Features => GetValue(x => x.Features);
}