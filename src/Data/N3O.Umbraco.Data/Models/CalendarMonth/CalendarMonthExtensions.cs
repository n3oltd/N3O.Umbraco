using NodaTime;
using System;
using System.Globalization;

namespace N3O.Umbraco.Data.Models; 

public static class CalendarMonthExtensions {
    public static LocalDate EndOfMonth(this CalendarMonth calendarMonth) {
        return new LocalDate(calendarMonth.Year,
                             calendarMonth.Month,
                             DateTime.DaysInMonth(calendarMonth.Year, calendarMonth.Month));
    }
    
    public static LocalDate StartOfMonth(this CalendarMonth calendarMonth) {
        return new LocalDate(calendarMonth.Year, calendarMonth.Month, 1);
    }

    public static object ToJsonObject(this CalendarMonth calendarMonth) {
        return new {
            year = calendarMonth.Year,
            month = calendarMonth.Month
        };
    }

    public static long ToLong(this CalendarMonth calendarMonth) {
        var str = $"{calendarMonth.Year.ToString(CultureInfo.InvariantCulture)}{calendarMonth.Month.ToString("00", CultureInfo.InvariantCulture)}";

        return long.Parse(str);
    }
}
