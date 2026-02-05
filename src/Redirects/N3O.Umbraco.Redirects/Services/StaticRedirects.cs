using N3O.Umbraco.Redirects.Models;
using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Redirects;

public static class StaticRedirects {
    private static readonly Dictionary<string, Redirect> Redirects = new(StringComparer.InvariantCultureIgnoreCase);

    public static void Add(string oldPath, string newPath, bool temporary = false) {
        Redirects.Add(Normalize(oldPath), new Redirect(temporary, $"/{Normalize(newPath)}"));
    }

    public static void Clear() {
        Redirects.Clear();
    }
    
    public static Redirect Find(string path) {
        return Redirects.GetValueOrDefault(Normalize(path));
    }
    
    private static string Normalize(string path) {
        return path.TrimStart('/').TrimEnd('/');
    }
}
