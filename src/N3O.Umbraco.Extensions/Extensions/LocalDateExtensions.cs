using NodaTime;
using System;

namespace N3O.Umbraco.Extensions {
    public static class LocalDateExtensions {
        public static LocalDate EndOfMonth(this LocalDate date) {
            var daysInMonth = DateTime.DaysInMonth(date.Year, date.Month);

            return new LocalDate(date.Year, date.Month, daysInMonth);
        }
    
        public static LocalDate EndOfWeek(this LocalDate date, IsoDayOfWeek weekEndsOn = IsoDayOfWeek.Sunday) {
            var daysToAdd = weekEndsOn - date.DayOfWeek;
            var endOfWeek = date.PlusDays(daysToAdd);

            return endOfWeek;
        }
    
        public static LocalDate EndOfYear(this LocalDate date) {
            return new LocalDate(date.Year, 12, 31);
        }

        public static LocalDate StartOfMonth(this LocalDate date) {
            return new LocalDate(date.Year, date.Month, 1);
        }
    
        public static LocalDate StartOfWeek(this LocalDate date, IsoDayOfWeek weekStartsOn = IsoDayOfWeek.Monday) {
            var daysSinceStartOfWeek = date.DayOfWeek - weekStartsOn;
            var startOfWeek = date.PlusDays(-daysSinceStartOfWeek);

            return startOfWeek;
        }
    
        public static LocalDate StartOfYear(this LocalDate date) {
            return new LocalDate(date.Year, 1, 1);
        }
    
        public static string ToYearMonthDayString(this LocalDate date) {
            return $"{date.Year}-{date.Month:00}-{date.Day:00}";
        }
    }
}
