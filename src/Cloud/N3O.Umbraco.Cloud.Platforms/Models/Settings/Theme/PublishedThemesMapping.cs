using N3O.Umbraco.Cloud.Platforms.Content;
using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Extensions;
using System.Collections.Generic;
using Umbraco.Cms.Core.Mapping;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class PublishedThemesMapping : IMapDefinition {
    public void DefineMaps(IUmbracoMapper mapper) {
        mapper.Define<ThemeSettingsContent, PublishedThemes>((_, _) => new PublishedThemes(), Map);
    }

    // Umbraco.Code.MapAll
    private void Map(ThemeSettingsContent src, PublishedThemes dest, MapperContext ctx) {
        dest.Default = ctx.Map<ThemeContent, PublishedTheme>(src.DefaultTheme);

        var additionalThemes = new Dictionary<string, PublishedTheme>();
        
        foreach (var additionalTheme in src.AdditionalThemes.OrEmpty()) {
            additionalThemes[additionalTheme.ThemeName] = ctx.Map<ThemeContent, PublishedTheme>(additionalTheme);
        }
        
        dest.Additional =  additionalThemes;
    }
}