using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace N3O.Umbraco.ContentFinders;

public static class SpecialContentPathParser {
    private static readonly ConcurrentDictionary<SpecialContent, IReadOnlyList<string>> SpecialPaths = new();

    public static void Flush() {
        SpecialPaths.Clear();
    }

    public static string GetPath(IContentCache contentCache, SpecialContent specialContent) {
        var specialPage = contentCache.Special(specialContent);

        if (specialPage.HasValue()) {
            var ambientPath = specialPage.RelativeUrl().StripTrailingSlash();

            if (IsRoutable(ambientPath)) {
                return ambientPath;
            }
        }

        return GetPaths(contentCache, specialContent).FirstOrDefault();
    }

    public static string ParseUri(IContentCache contentCache, SpecialContent specialContent, Uri requestUri) {
        var requestedPath = requestUri.GetAbsolutePathDecoded().ToLowerInvariant().StripTrailingSlash();

        foreach (var specialPath in GetPaths(contentCache, specialContent).OrderByDescending(x => x.Length)) {
            if (requestedPath.StartsWith(specialPath, StringComparison.InvariantCultureIgnoreCase)) {
                return requestedPath.Substring(specialPath.Length).EnsureTrailingSlash();
            }
        }

        return null;
    }

    private static void AddPathIfRoutable(ICollection<string> specialPaths, string url) {
        if (!url.HasValue()) {
            return;
        }

        var path = url.StripTrailingSlash();

        if (IsRoutable(path) && !specialPaths.Contains(path, StringComparer.InvariantCultureIgnoreCase)) {
            specialPaths.Add(path);
        }
    }

    private static IReadOnlyList<string> GetCultures(IPublishedContent specialPage) {
        return specialPage.AncestorsOrSelf()
                          .SelectMany(x => x.Cultures.Keys)
                          .Distinct(StringComparer.InvariantCultureIgnoreCase)
                          .ToList();
    }

    private static IReadOnlyList<string> GetPaths(IContentCache contentCache, SpecialContent specialContent) {
        if (SpecialPaths.TryGetValue(specialContent, out var cachedPaths)) {
            return cachedPaths;
        }

        var specialPage = contentCache.Special(specialContent);

        if (!specialPage.HasValue()) {
            return [];
        }

        var specialPaths = GetRoutablePaths(specialPage);

        if (specialPaths.None()) {
            return [];
        }

        SpecialPaths.TryAdd(specialContent, specialPaths);

        return specialPaths;
    }

    private static IReadOnlyList<string> GetRoutablePaths(IPublishedContent specialPage) {
        var specialPaths = new List<string>();

        AddPathIfRoutable(specialPaths, specialPage.RelativeUrl());

        foreach (var culture in GetCultures(specialPage)) {
            AddPathIfRoutable(specialPaths, specialPage.Url(culture, UrlMode.Relative));
        }

        return specialPaths;
    }

    private static bool IsRoutable(string path) {
        return path.HasValue() && path.StartsWith('/');
    }
}
