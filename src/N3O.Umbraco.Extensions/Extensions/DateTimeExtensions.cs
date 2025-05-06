using N3O.Umbraco.Localization;
using NodaTime;
using NodaTime.Extensions;
using System;
using System.Globalization;

namespace N3O.Umbraco.Extensions;

public static class DateTimeExtensions {
    public static bool HasValue(this DateTime dt) {
        return HasValue((DateTime?) dt);
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

        return zonedDateTime;
    }
    
    public static DateTime ToHijriDateTime(this DateTime dateTime, int offset = 0) {
        var hijriCalendar = new HijriCalendar();
        hijriCalendar.HijriAdjustment = offset;

        var day = hijriCalendar.GetDayOfMonth(dateTime);
        var month = hijriCalendar.GetMonth(dateTime);
        var year = hijriCalendar.GetYear(dateTime);

        var hour = hijriCalendar.GetHour(dateTime);
        var minute = hijriCalendar.GetMinute(dateTime);
        var second = hijriCalendar.GetSecond(dateTime);
        var milliseconds = (int) hijriCalendar.GetMilliseconds(dateTime);

        return new DateTime(year, month, day, hour, minute, second, milliseconds);
    }

    public static LocalDate ToLocalDate(this DateTime dateTime) {
        return dateTime.ToLocalDateTime().Date;
    }
}