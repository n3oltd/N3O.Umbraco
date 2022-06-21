using N3O.Umbraco.Localization;
using NodaTime;
using NodaTime.Extensions;
using System;

namespace N3O.Umbraco.Extensions;

public static class DateTimeOffsetExtensions {
    public static bool HasValue(this DateTimeOffset dt) {
        return HasValue((DateTimeOffset?)dt);
    }

    public static bool HasValue(this DateTimeOffset? dt) {
        if (dt == null || dt.Value == default) {
            return false;
        }

        return true;
    }

    public static LocalDate ToLocalDate(this DateTimeOffset dateTimeOffset, ILocalClock localClock) {
        return dateTimeOffset.ToLocalDateTime(localClock).Date;
    }
    
    public static LocalDateTime ToLocalDateTime(this DateTimeOffset dateTimeOffset, ILocalClock localClock) {
        return localClock.ToLocalDateTime(dateTimeOffset.ToInstant());
    }
}
