using System;
using System.Collections.Generic;

namespace N3O.Umbraco.Hosting;

public static class StaticRedirects {
    private static readonly Dictionary<string, StaticRedirect> Redirects = new(StringComparer.InvariantCultureIgnoreCase);

    public static void Add(string oldPath, string newPath, bool temporary = false) {
        Redirects.Add(Normalize(oldPath), new StaticRedirect($"/{Normalize(newPath)}", temporary));
    }

    public static StaticRedirect Find(string path) {
        return Redirects.GetValueOrDefault(Normalize(path));
    }
    
    private static string Normalize(string path) {
        return path.TrimStart('/').TrimEnd('/');
    }
}
