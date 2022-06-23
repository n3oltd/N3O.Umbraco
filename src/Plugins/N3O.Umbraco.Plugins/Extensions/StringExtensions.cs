using NodaTime;
using System.IO;

namespace N3O.Umbraco.Plugins.Extensions;

public static class StringExtensions {
    public static string GetStoragePath(this string filename, string instant) {
        return Path.Combine(instant, filename);
    }

    public static string GetMediaUrlPath(this string filename, string instant) {
        return $"/media/{GetStoragePath(filename, instant).Replace("\\", "/")}";
    }
}
