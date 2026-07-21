using N3O.Umbraco.Extensions;
using N3O.Umbraco.Search.Models;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Search;

public static class SitemapSections {
    private static readonly Dictionary<string, string> SectionsByContentTypeAlias = new(StringComparer.OrdinalIgnoreCase);

    public static void RegisterSection(string section, params string[] contentTypeAliases) {
        foreach (var contentTypeAlias in contentTypeAliases.OrEmpty()) {
            SectionsByContentTypeAlias[contentTypeAlias] = section;
        }
    }

    public static string GetSection(string contentTypeAlias) {
        if (contentTypeAlias.HasValue() && SectionsByContentTypeAlias.TryGetValue(contentTypeAlias, out var section)) {
            return section;
        }

        return SitemapEntry.DefaultSection;
    }
}
