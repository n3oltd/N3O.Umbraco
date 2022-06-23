using NodaTime;
using System.Globalization;

namespace N3O.Umbraco.Plugins.Extensions;

public static class InstantExtensions {
    public static string GetMediaId(this Instant instant) {
        return instant.ToUnixTimeTicks().ToString(CultureInfo.InvariantCulture);
    }
}
