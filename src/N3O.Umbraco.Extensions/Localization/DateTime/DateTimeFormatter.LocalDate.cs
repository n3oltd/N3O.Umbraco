using NodaTime;
using System;

namespace N3O.Umbraco.Localization;

public partial class DateTimeFormatter {
    public string FormatDate(LocalDate? localDate, DateFormat dateFormat = null) {
        return Format(localDate, dateFormat, FormatDate);
    }

    public string FormatDate(LocalDate? localDate, string specifier) {
        return Format(localDate, (DateFormat) null, (dt, _) => FormatDate(dt, specifier));
    }

    private string Format<TDateOrTimeFormat>(LocalDate? localDate,
                                             TDateOrTimeFormat format,
                                             Func<DateTime?, TDateOrTimeFormat, string> formatter) {
        if (localDate == null) {
            return null;
        }

        var dateTime = localDate.Value.ToDateTimeUnspecified();

        return formatter(dateTime, format);
    }
}
