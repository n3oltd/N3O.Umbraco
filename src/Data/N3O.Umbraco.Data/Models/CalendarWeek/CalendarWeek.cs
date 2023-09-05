using N3O.Umbraco.Data.Extensions;
using N3O.Umbraco.Extensions;
using NodaTime;
using System;

namespace N3O.Umbraco.Data.Models; 

public class CalendarWeek : Value, IComparable<CalendarWeek> {
    public CalendarWeek(int week, int year) {
        Week = week;
        Year = year;
    }

    public int Week { get; }
    public int Year { get; }

    public CalendarWeek MinusWeeks(int weeks) {
        return this.StartOfWeek().PlusWeeks(-weeks).ToCalendarWeek();
    }
    
    public CalendarWeek PlusWeeks(int weeks) {
        return this.StartOfWeek().PlusWeeks(weeks).ToCalendarWeek();
    }
    
    public CalendarWeek MinusYears(int years) {
        return this.StartOfWeek().PlusYears(-years).ToCalendarWeek();
    }
    
    public CalendarWeek PlusYears(int years) {
        return this.StartOfWeek().PlusYears(years).ToCalendarWeek();
    }

    public override string ToString() {
        return $"{Week}/{Year}";
    }

    public int CompareTo(CalendarWeek other) {
        int result;
        
        if (other == null) {
            result = 1;
        } else {
            var asString = $"{Year}{Week}";
            var otherAsString = $"{other.Year}{other.Week}";
            
            result = string.Compare(asString, otherAsString, StringComparison.InvariantCultureIgnoreCase); 
        }

        return result;
    }
    
    public static bool operator <(CalendarWeek lhs, CalendarWeek rhs) {
        if (lhs.Year < rhs.Year) {
            return true;
        } else  if (lhs.Year > rhs.Year) {
            return false;
        }

        return lhs.Week < rhs.Week;
    }

    public static bool operator <=(CalendarWeek lhs, CalendarWeek rhs) {
        if (lhs.Year < rhs.Year) {
            return true;
        } else if (lhs.Year > rhs.Year) {
            return false;
        }

        return lhs.Week <= rhs.Week;
    }

    public static bool operator >(CalendarWeek lhs, CalendarWeek rhs) {
        if (lhs.Year > rhs.Year) {
            return true;
        } else if (lhs.Year < rhs.Year) {
            return false;
        }

        return lhs.Week > rhs.Week;
    }

    public static bool operator >=(CalendarWeek lhs, CalendarWeek rhs) {
        if (lhs.Year < rhs.Year) {
            return false;
        } else if (lhs.Year > rhs.Year) {
            return true;
        }

        return lhs.Week >= rhs.Week;
    }

    public static explicit operator CalendarWeek(DateTime date) {
        return FromDate(date);
    }
    
    public static explicit operator CalendarWeek(DateTime? date) {
        return FromDate(date);
    }
    
    public static explicit operator CalendarWeek(LocalDate date) {
        return FromDate(date.ToDateTimeUnspecified());
    }
    
    public static explicit operator CalendarWeek(LocalDate? date) {
        return FromDate(date?.ToDateTimeUnspecified());
    }
    
    public static explicit operator CalendarWeek(LocalDateTime date) {
        return FromDate(date.ToDateTimeUnspecified());
    }
    
    public static explicit operator CalendarWeek(LocalDateTime? date) {
        return FromDate(date?.ToDateTimeUnspecified());
    }
    
    private static CalendarWeek FromDate(DateTime? date) {
        if (date == null) {
            return null;
        }

        return date.Value.ToLocalDate().ToCalendarWeek();
    }
}
