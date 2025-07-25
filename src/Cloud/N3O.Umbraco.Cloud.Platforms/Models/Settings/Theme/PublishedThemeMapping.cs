﻿using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Cloud.Platforms.Clients;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class PublishedThemeMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<ThemeSettingsContent, PublishedTheme>((_, _) => new PublishedTheme(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(ThemeSettingsContent src, PublishedTheme dest, MapperContext ctx) {
        dest.BorderRadius = src.BorderRadius;
        dest.FontFamily = src.FontFamily;
        dest.HeadingFontFamily = src.HeadingFontFamily;
        
        dest.Colors = new PublishedThemeColors();
        dest.Colors.Background = src.Background;
        dest.Colors.Foreground = src.Foreground;
        dest.Colors.Primary = src.Primary;
        dest.Colors.PrimaryForeground = src.PrimaryForeground;
        dest.Colors.Secondary = src.Secondary;
        dest.Colors.SecondaryForeground = src.SecondaryForeground;
        dest.Colors.Accent = src.Accent;
        dest.Colors.AccentForeground = src.AccentForeground;
        dest.Colors.Muted = src.Muted;
        dest.Colors.MutedForeground = src.MutedForeground;
        dest.Colors.Card = src.Card;
        dest.Colors.CardForeground = src.CardForeground;
        dest.Colors.Destructive = src.Destructive;
        dest.Colors.DestructiveForeground = src.DestructiveForeground;
        dest.Colors.Border = src.Border;
        dest.Colors.Input = src.Input;
        dest.Colors.Ring = src.Ring;
        dest.Colors.Chart1 = src.Chart1;
        dest.Colors.Chart2 = src.Chart2;
        dest.Colors.Chart3 = src.Chart3;
        dest.Colors.Chart4 = src.Chart4;
        dest.Colors.Chart5 = src.Chart5;
        dest.Colors.Popover = src.Popover;
        dest.Colors.PopoverForeground = src.PopoverForeground;
    }
}