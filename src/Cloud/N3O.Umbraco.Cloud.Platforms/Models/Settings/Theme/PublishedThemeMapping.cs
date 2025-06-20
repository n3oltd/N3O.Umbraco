using N3O.Umbraco.Cloud.Platforms.Content;
using MuslimHands.Website.Connect.Clients;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class PublishedThemeMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<PlatformsThemeSettings, PublishedTheme>((_, _) => new PublishedTheme(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(PlatformsThemeSettings src, PublishedTheme dest, MapperContext ctx) {
        dest.BorderRadius = src.BorderRadius;
        dest.FontFamily = src.FontFamily;
        dest.HeadingFontFamily = src.HeadingFontFamily;
        
        dest.Colors = new PublishedThemeColors();
        dest.Colors.Accent = src.Accent;
        dest.Colors.AccentForeground = src.Accent;
        dest.Colors.Background = src.Accent;
        dest.Colors.Border = src.Accent;
        dest.Colors.Card = src.Accent;
        dest.Colors.Card = src.Accent;
        dest.Colors.Destructive = src.Accent;
        dest.Colors.DestructiveForeground = src.Accent;
        dest.Colors.Foreground = src.Foreground;
        dest.Colors.Input = src.Input;
        dest.Colors.Muted = src.Muted;
        dest.Colors.MutedForeground = src.MutedForeground;
        dest.Colors.Primary = src.Primary;
        dest.Colors.Primary = src.Primary;
        dest.Colors.PrimaryForeground = src.PrimaryForeground;
        dest.Colors.Ring = src.Ring;
        dest.Colors.Secondary = src.Secondary;
        dest.Colors.SecondaryForeground = src.SecondaryForeground;
    }
}