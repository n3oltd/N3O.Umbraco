using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Extensions;
using System.Collections.Generic;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class UmbracoThemesReqMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<ThemeSettingsContent, UmbracoThemesReq>((_, _) => new UmbracoThemesReq(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(ThemeSettingsContent src, UmbracoThemesReq dest, MapperContext ctx) {
        dest.Default = ctx.Map<ThemeContent, UmbracoThemeReq>(src.DefaultTheme);

        var additionalThemes = new Dictionary<string, UmbracoThemeReq>();
        
        foreach (var additionalTheme in src.AdditionalThemes.OrEmpty()) {
            additionalThemes[additionalTheme.ThemeName] = ctx.Map<ThemeContent, UmbracoThemeReq>(additionalTheme);
        }
        
        dest.Additional =  additionalThemes;
    }
}