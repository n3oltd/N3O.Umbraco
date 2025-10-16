using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Media;
using System;
using System.Linq;
using Umbraco.Cms.Core.Mapping;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class UmbracoThemeReqMapping : IMapDefinition {
    private readonly IMediaUrl _mediaUrl;

    public UmbracoThemeReqMapping(IMediaUrl mediaUrl) {
        _mediaUrl = mediaUrl;
    }
    
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<ThemeContent, UmbracoThemeReq>((_, _) => new UmbracoThemeReq(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(ThemeContent src, UmbracoThemeReq dest, MapperContext ctx) {
        dest.Settings = GetSettings(src);
        dest.Css = src.Css;
        dest.CssImports = src.CssImports.ToList();
        dest.FontImports = src.FontImports.ToList();
    }

    private PublishedTheme GetSettings(ThemeContent src) {
        var theme  = new PublishedTheme();
        
        theme.BorderRadius = src.BorderRadius;
        theme.FontFamily = src.FontFamily;
        theme.HeadingFontFamily = src.HeadingFontFamily;
        
        theme.Colors = new PublishedThemeColors();
        theme.Colors.Background = src.Background;
        theme.Colors.Foreground = src.Foreground;
        theme.Colors.Primary = src.Primary;
        theme.Colors.PrimaryForeground = src.PrimaryForeground;
        theme.Colors.Secondary = src.Secondary;
        theme.Colors.SecondaryForeground = src.SecondaryForeground;
        theme.Colors.Accent = src.Accent;
        theme.Colors.AccentForeground = src.AccentForeground;
        theme.Colors.Muted = src.Muted;
        theme.Colors.MutedForeground = src.MutedForeground;
        theme.Colors.Card = src.Card;
        theme.Colors.CardForeground = src.CardForeground;
        theme.Colors.Destructive = src.Destructive;
        theme.Colors.DestructiveForeground = src.DestructiveForeground;
        theme.Colors.Border = src.Border;
        theme.Colors.Input = src.Input;
        theme.Colors.Ring = src.Ring;
        theme.Colors.Chart1 = src.Chart1;
        theme.Colors.Chart2 = src.Chart2;
        theme.Colors.Chart3 = src.Chart3;
        theme.Colors.Chart4 = src.Chart4;
        theme.Colors.Chart5 = src.Chart5;
        theme.Colors.Popover = src.Popover;
        theme.Colors.PopoverForeground = src.PopoverForeground;
        theme.Colors.IconBackgroundColor = src.IconBackgroundColor;
        theme.Colors.IconBackgroundColorDark = src.IconBackgroundColorDark;
        theme.Colors.SplashBackgroundColor = src.SplashBackgroundColor;
        theme.Colors.SplashBackgroundColorDark = src.SplashBackgroundColorDark;
        
        theme.MobileApp = new PublishedThemeMobileApp();
        theme.MobileApp.Logo = GetPublishedThemeMobileAppAsset(MobileAppAssetType.Logo, src.Logo);
        theme.MobileApp.LogoDark  = GetPublishedThemeMobileAppAsset(MobileAppAssetType.LogoDark, src.LogoDark);
        
        return theme;
    }

    private PublishedThemeMobileAppAsset GetPublishedThemeMobileAppAsset(MobileAppAssetType assetType, MediaWithCrops image) {
        var asset = new PublishedThemeMobileAppAsset();
        asset.Type = assetType;
        asset.Url = _mediaUrl.GetMediaUrl(image, urlMode: UrlMode.Absolute).IfNotNull(x => new Uri(x));
        
        return asset;
    }
}