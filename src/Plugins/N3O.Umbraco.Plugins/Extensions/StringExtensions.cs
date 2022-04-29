using NodaTime;
using System.Globalization;
using System.IO;

namespace N3O.Umbraco.Plugins.Extensions {
    public static class StringExtensions {
        public static string GetStoragePath(this string filename, Instant instant) {
            return Path.Combine(instant.ToString("yyMMddHHmmss", CultureInfo.InvariantCulture), filename);
        }

        public static string GetMediaPath(this string filename, Instant instant) {
            return $"/media/{GetStoragePath(filename, instant).Replace("\\", "/")}";
        }
    }
}