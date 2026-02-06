using N3O.Umbraco.Redirects.Models;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Redirects;

public static class UmbracoRedirects {
    private static readonly Dictionary<string, Redirect> Redirects = new(StringComparer.InvariantCultureIgnoreCase);

    public static void Add(string oldPath, string newUrlOrPath, bool temporary = false) {
        Redirects.Add(Normalize(oldPath), new Redirect(temporary, GetUrlOrPath(newUrlOrPath)));
    }
    
    public static void Clear() {
        Redirects.Clear();
    }

    public static Redirect Find(string path) {
        return Redirects.GetValueOrDefault(Normalize(path));
    }

    private static string GetUrlOrPath(string urlOrPath) {
        if (Uri.TryCreate(urlOrPath, UriKind.Absolute, out _)) {
            return urlOrPath;
        } else {
            return $"/{Normalize(urlOrPath)}";
        }
    }
    
    private static string Normalize(string path) {
        return path.TrimStart('/').TrimEnd('/');
    }
}
