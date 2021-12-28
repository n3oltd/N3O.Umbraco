using NodaTime;
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
    
    public static LocalDate ToLocalDate(this DateTime dateTime) {
        return new LocalDate(dateTime.Year, dateTime.Month, dateTime.Day);
    }
}
