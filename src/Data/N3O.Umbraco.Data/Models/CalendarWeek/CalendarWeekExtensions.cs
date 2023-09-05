using NodaTime;
using NodaTime.Calendars;
using System.Globalization;

namespace N3O.Umbraco.Data.Models; 

public static class CalendarWeekExtensions {
    public static LocalDate EndOfWeek(this CalendarWeek calendarWeek) {
        return WeekYearRules.Iso.GetLocalDate(calendarWeek.Year, calendarWeek.Week, IsoDayOfWeek.Sunday);
    }
    
    public static LocalDate StartOfWeek(this CalendarWeek calendarWeek) {
        return WeekYearRules.Iso.GetLocalDate(calendarWeek.Year, calendarWeek.Week, IsoDayOfWeek.Monday);
    }

    public static object ToJsonObject(this CalendarWeek calendarWeek) {
        return new {
            year = calendarWeek.Year,
            week = calendarWeek.Week
        };
    }

    public static long ToLong(this CalendarWeek calendarWeek) {
        var str = $"{calendarWeek.Year.ToString(CultureInfo.InvariantCulture)}{calendarWeek.Week.ToString("00", CultureInfo.InvariantCulture)}";

        return long.Parse(str);
    }
}
