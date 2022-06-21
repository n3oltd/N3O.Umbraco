using NodaTime;
using System.IO;

namespace N3O.Umbraco.Plugins.Extensions;

public static class StringExtensions {
    public static string GetStoragePath(this string filename, Instant instant) {
        return Path.Combine(instant.GetMediaId(), filename);
    }

    public static string GetMediaUrlPath(this string filename, Instant instant) {
        return $"/media/{GetStoragePath(filename, instant).Replace("\\", "/")}";
    }
}
