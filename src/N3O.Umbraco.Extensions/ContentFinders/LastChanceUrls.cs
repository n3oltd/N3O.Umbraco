using Microsoft.Extensions.Logging;
using N3O.Umbraco.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.ContentFinders;

public static class LastChanceUrls {
    private static readonly Dictionary<string, LastChanceUrl> Redirects = new(StringComparer.InvariantCultureIgnoreCase);

    public static void Add(string oldPath, string newPath, bool temporary = false) {
        Redirects.Add(Normalize(oldPath), new LastChanceUrl(Normalize(newPath), temporary));
    }

    public static LastChanceUrl Find(ILogger<LastChanceFinder> logger, string path) {
        logger.LogInformation("Path requested: {Path}", path);
        logger.LogInformation("Paths Added: {Paths}", Redirects.GetValues().Join(","));
        
        return Redirects.GetValueOrDefault(Normalize(path));
    }
    
    private static string Normalize(string path) {
        return path.TrimStart('/').TrimEnd('/');
    }
}
