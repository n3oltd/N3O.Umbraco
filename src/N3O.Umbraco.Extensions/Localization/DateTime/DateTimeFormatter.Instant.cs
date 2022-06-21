using NodaTime;
using System;

namespace N3O.Umbraco.Localization;

public partial class DateTimeFormatter {
    public string FormatDate(Instant? instant, DateFormat dateFormat = null) {
        return Format(instant, dateFormat, FormatDate);
    }

    public string FormatDate(Instant? instant, string specifier) {
        return Format(instant, (DateFormat) null, (dt, _) => FormatDate(dt, specifier));
    }

    public string FormatTime(Instant? instant, TimeFormat timeFormat = null) {
        return Format(instant, timeFormat, FormatTime);
    }

    public string FormatTime(Instant? instant, string specifier) {
        return Format(instant, (DateFormat) null, (dt, _) => FormatTime(dt, specifier));
    }

    public string FormatDateWithTime(Instant? instant) {
        if (instant == null) {
            return null;
        }

        return $"{FormatDate(instant)} {FormatTime(instant)}";
    }

    private string Format<TDateOrTimeFormat>(Instant? instant, TDateOrTimeFormat format, Func<DateTime?, TDateOrTimeFormat, string> formatter) {
        if (instant == null) {
            return null;
        }

        var timezone = GetDateTimezone();

        var zonedDateTime = instant.Value.InZone(timezone);
        var localDateTime = zonedDateTime.LocalDateTime;

        var dateTime = localDateTime.ToDateTimeUnspecified();

        return formatter(dateTime, format);
    }
}
