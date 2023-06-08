using NodaTime;
using System;
using System.Globalization;

namespace N3O.Umbraco.Localization;

public partial class DateTimeFormatter {
    public string FormatDate(DateTime? dateTime, DateFormat dateFormat = null) {
        if (dateTime == null) {
            return null;
        }

        var localFormat = GetDateFormatInfo(null);

        return dateTime.Value.ToString("d", localFormat);
    }

    public string FormatDate(DateTime? dateTime, string specifier) {
        var localFormat = GetDateFormatInfo(null);
    
        return dateTime?.ToString(specifier, localFormat);
    }

    public string FormatTime(DateTime? dateTime, TimeFormat timeFormat = null) {
        if (dateTime == null) {
            return null;
        }

        var localFormat = GetDateFormatInfo(null);

        return dateTime.Value.ToString("t", localFormat);
    }

    public string FormatTime(DateTime? dateTime, string specifier) {
        var localFormat = GetDateFormatInfo(null);

        return dateTime?.ToString(specifier, localFormat);
    }

    public string FormatDateWithTime(DateTime? dateTime) {
        if (dateTime == null) {
            return null;
        }

        return $"{FormatDate(dateTime)} {FormatTime(dateTime)}";
    }

    private DateTimeZone GetDateTimezone() {
        var timezone = _settingsAccessor.GetSettings().Timezone;

        return timezone.Zone;
    }

    private DateTimeFormatInfo GetDateFormatInfo(DateFormat dateFormat) {
        if (dateFormat == null) {
            dateFormat = _settingsAccessor.GetSettings().DateFormat;
        }

        var dateTimeFormatInfo = dateFormat.GetDateTimeFormatInfo();

        return dateTimeFormatInfo;
    }
}
