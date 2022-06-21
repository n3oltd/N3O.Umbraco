using NodaTime;
using System;

namespace N3O.Umbraco.Extensions;

public static class YearMonthExtensions {
    public static LocalDate EndOfMonth(this YearMonth yearMonth) {
        return new LocalDate(yearMonth.Year,
                             yearMonth.Month,
                             DateTime.DaysInMonth(yearMonth.Year, yearMonth.Month));
    }

    public static LocalDate StartOfMonth(this YearMonth yearMonth) {
        return new LocalDate(yearMonth.Year, yearMonth.Month, 1);
    }
}
