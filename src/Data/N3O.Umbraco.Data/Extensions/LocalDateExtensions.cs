using N3O.Umbraco.Data.Models;
using NodaTime;
using NodaTime.Calendars;

namespace N3O.Umbraco.Data.Extensions; 

public static class LocalDateExtensions {
    public static CalendarMonth ToCalendarMonth(this LocalDate date) {
        return new CalendarMonth(date.Month, date.Year);
    }
    
    public static CalendarWeek ToCalendarWeek(this LocalDate date) {
        var year = WeekYearRules.Iso.GetWeekYear(date);
        var week = WeekYearRules.Iso.GetWeekOfWeekYear(date);

        return new CalendarWeek(week, year);
    }
}