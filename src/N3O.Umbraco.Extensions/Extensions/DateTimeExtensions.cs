using N3O.Umbraco.Localization;
using NodaTime;
using NodaTime.Extensions;
using System;

namespace N3O.Umbraco.Extensions;

public static class DateTimeExtensions {
    public static bool HasValue(this DateTime dt) {
        return HasValue((DateTime?)dt);
    }

    public static bool HasValue(this DateTime? dt) {
        if (dt == null || dt.Value == default) {
            return false;
        }

        return true;
    }
    
    public static ZonedDateTime InTimezone(this DateTime dateTime, Timezone timezone) {
        if (dateTime.Kind != DateTimeKind.Utc) {
            dateTime = DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
        }

        var zonedDateTime = new ZonedDateTime(Instant.FromDateTimeUtc(dateTime), timezone.Zone);
    
        return  zonedDateTime;
    }
    
    public static LocalDate ToLocalDate(this DateTime dateTime) {
        return dateTime.ToLocalDateTime().Date;
    }
}
