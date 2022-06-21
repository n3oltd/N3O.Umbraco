using NodaTime;
using System;

namespace N3O.Umbraco.Extensions;

public static class ClockExtensions {
    public static DateTime GetUtcNow(this IClock clock) {
        return clock.GetCurrentInstant().ToDateTimeUtc();
    }
}
