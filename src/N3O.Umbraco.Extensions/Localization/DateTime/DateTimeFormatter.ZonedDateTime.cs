using NodaTime;
using System;

namespace N3O.Umbraco.Localization;

public partial class DateTimeFormatter {
    public string FormatDate(ZonedDateTime? zonedDateTime, DateFormat dateFormat = null) {
        return Format(zonedDateTime, dateFormat, FormatDate);
    }

    public string FormatDate(ZonedDateTime? zonedDateTime, string specifier) {
        return Format(zonedDateTime, (DateFormat) null, (dt, _) => FormatDate(dt, specifier));
    }

    public string FormatTime(ZonedDateTime? zonedDateTime, TimeFormat timeFormat = null) {
        return Format(zonedDateTime, timeFormat, FormatTime);
    }

    public string FormatTime(ZonedDateTime? zonedDateTime, string specifier) {
        return Format(zonedDateTime, (DateFormat) null, (dt, _) => FormatTime(dt, specifier));
    }

    public string FormatDateWithTime(ZonedDateTime? zonedDateTime) {
        if (zonedDateTime == null) {
            return null;
        }

        return $"{FormatDate(zonedDateTime)} {FormatTime(zonedDateTime)}";
    }

    private string Format<TDateOrTimeFormat>(ZonedDateTime? zonedDateTime, TDateOrTimeFormat format, Func<DateTime?, TDateOrTimeFormat, string> formatter) {
        if (zonedDateTime == null) {
            return null;
        }

        var dateTime = zonedDateTime.Value.ToDateTimeUnspecified();

        return formatter(dateTime, format);
    }
}
