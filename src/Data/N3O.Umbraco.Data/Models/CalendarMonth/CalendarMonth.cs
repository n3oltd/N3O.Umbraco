using N3O.Umbraco.Data.Extensions;
using NodaTime;
using System;

namespace N3O.Umbraco.Data.Models; 

public class CalendarMonth : Value, IComparable<CalendarMonth> {
    public CalendarMonth(int month, int year) {
        Month = month;
        Year = year;
    }

    public int Month { get; }
    public int Year { get; }

    public CalendarMonth MinusMonths(int months) {
        return this.StartOfMonth().PlusMonths(-months).ToCalendarMonth();
    }
    
    public CalendarMonth PlusMonths(int months) {
        return this.StartOfMonth().PlusMonths(months).ToCalendarMonth();
    }
    
    public CalendarMonth MinusYears(int years) {
        return this.StartOfMonth().PlusYears(-years).ToCalendarMonth();
    }
    
    public CalendarMonth PlusYears(int years) {
        return this.StartOfMonth().PlusYears(years).ToCalendarMonth();
    }

    public override string ToString() {
        return $"{Month}/{Year}";
    }

    public int CompareTo(CalendarMonth other) {
        int result;
        
        if (other == null) {
            result = 1;
        } else {
            var asString = $"{Year}{Month}";
            var otherAsString = $"{other.Year}{other.Month}";
            
            result = string.Compare(asString, otherAsString, StringComparison.InvariantCultureIgnoreCase); 
        }

        return result;
    }
    
    public static bool operator <(CalendarMonth lhs, CalendarMonth rhs) {
        if (lhs.Year < rhs.Year) {
            return true;
        } else  if (lhs.Year > rhs.Year) {
            return false;
        }

        return lhs.Month < rhs.Month;
    }

    public static bool operator <=(CalendarMonth lhs, CalendarMonth rhs) {
        if (lhs.Year < rhs.Year) {
            return true;
        } else if (lhs.Year > rhs.Year) {
            return false;
        }

        return lhs.Month <= rhs.Month;
    }

    public static bool operator >(CalendarMonth lhs, CalendarMonth rhs) {
        if (lhs.Year > rhs.Year) {
            return true;
        } else if (lhs.Year < rhs.Year) {
            return false;
        }

        return lhs.Month > rhs.Month;
    }

    public static bool operator >=(CalendarMonth lhs, CalendarMonth rhs) {
        if (lhs.Year < rhs.Year) {
            return false;
        } else if (lhs.Year > rhs.Year) {
            return true;
        }

        return lhs.Month >= rhs.Month;
    }
    
    public static implicit operator CalendarMonth(YearMonth yearMonth) {
        return new CalendarMonth(yearMonth.Month, yearMonth.Year);
    }
    
    public static implicit operator YearMonth(CalendarMonth calendarMonth) {
        return new YearMonth(calendarMonth.Year, calendarMonth.Month);
    }

    public static explicit operator CalendarMonth(DateTime date) {
        return FromDate(date);
    }
    
    public static explicit operator CalendarMonth(DateTime? date) {
        return FromDate(date);
    }
    
    public static explicit operator CalendarMonth(LocalDate date) {
        return FromDate(date.ToDateTimeUnspecified());
    }
    
    public static explicit operator CalendarMonth(LocalDate? date) {
        return FromDate(date?.ToDateTimeUnspecified());
    }
    
    public static explicit operator CalendarMonth(LocalDateTime date) {
        return FromDate(date.ToDateTimeUnspecified());
    }
    
    public static explicit operator CalendarMonth(LocalDateTime? date) {
        return FromDate(date?.ToDateTimeUnspecified());
    }
    
    private static CalendarMonth FromDate(DateTime? date) {
        if (date == null) {
            return null;
        }
        
        return new CalendarMonth(date.Value.Month, date.Value.Year);
    }
}
