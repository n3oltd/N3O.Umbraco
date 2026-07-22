using N3O.Umbraco.Localization;
using N3O.Umbraco.Lookups;
using NodaTime;
using NodaTime.Calendars;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace N3O.Umbraco.Lookups;

public class DayOfWeek : NamedLookup, IComparable<DayOfWeek>, IEquatable<DayOfWeek>, IComparable<int>, IEquatable<int> {
    public DayOfWeek(int day, string name) : base($"_{day}", name) {
        Day = day;
        IsoDayOfWeek = (IsoDayOfWeek) day;
    }

    public LocalDate GetDateIn(int year, int week) {
        return WeekYearRules.Iso.GetLocalDate(year, week, IsoDayOfWeek);
    }

    public LocalDate GetNextDateAfterToday(ILocalClock localClock) {
        return GetNextDate(localClock, (today, date) => today >= date);
    }

    public LocalDate GetNextDateOrToday(ILocalClock localClock) {
        return GetNextDate(localClock, (today, date) => today > date);
    }

    public string ToOrdinal(INumberFormatter numberFormatter) {
        return numberFormatter.FormatOrdinal(Day);
    }

    public int Day { get; }
    public IsoDayOfWeek IsoDayOfWeek { get; }

    public bool Equals(DayOfWeek other) {
        if (other == null) {
            return false;
        }

        return Equals(other.Day);
    }

    public bool Equals(int other) {
        return Equals(Day, other);
    }

    public override bool Equals(object obj) {
        if (ReferenceEquals(null, obj)) {
            return false;
        }

        if (ReferenceEquals(this, obj)) {
            return true;
        }

        return obj.GetType() == GetType() && Equals((DayOfWeek) obj);
    }

    public int CompareTo(DayOfWeek other) {
        return CompareTo(other.Day);
    }

    public int CompareTo(int other) {
        return Day.CompareTo(other);
    }

    public override int GetHashCode() {
        return Day.GetHashCode();
    }

    public static explicit operator IsoDayOfWeek(DayOfWeek dayOfWeek) {
        return dayOfWeek.IsoDayOfWeek;
    }

    public static bool operator <(DayOfWeek lhs, DayOfWeek rhs) {
        return lhs.Day < rhs.Day;
    }

    public static bool operator <=(DayOfWeek lhs, DayOfWeek rhs) {
        return lhs.Day <= rhs.Day;
    }

    public static bool operator >(DayOfWeek lhs, DayOfWeek rhs) {
        return lhs.Day > rhs.Day;
    }

    public static bool operator >=(DayOfWeek lhs, DayOfWeek rhs) {
        return lhs.Day >= rhs.Day;
    }

    private LocalDate GetNextDate(ILocalClock localClock,
                                       Func<LocalDate, LocalDate, bool> rollForwardPredicate) {
        var today = localClock.GetLocalToday();
        var year = WeekYearRules.Iso.GetWeekYear(today);
        var week = WeekYearRules.Iso.GetWeekOfWeekYear(today);

        var date = GetDateIn(year, week);

        var rollForward = rollForwardPredicate(today, date);

        if (rollForward) {
            date = date.PlusWeeks(1);
        }

        return date;
    }

    public override IEnumerable<string> GetTextValues() {
        foreach (var value in base.GetTextValues()) {
            yield return value;
        }

        yield return Day.ToString(CultureInfo.InvariantCulture);
    }
}

public class DaysOfWeek : StaticLookupsCollection<DayOfWeek> {
    public static readonly DayOfWeek Monday = new(1, "Monday");
    public static readonly DayOfWeek Tuesday = new(2, "Tuesday");
    public static readonly DayOfWeek Wednesday = new(3, "Wednesday");
    public static readonly DayOfWeek Thursday = new(4, "Thursday");
    public static readonly DayOfWeek Friday = new(5, "Friday");
    public static readonly DayOfWeek Saturday = new(6, "Saturday");
    public static readonly DayOfWeek Sunday = new(7, "Sunday");

    // Static fields are initialized in order
    public static readonly IReadOnlyList<DayOfWeek> All = [
        Monday, Tuesday, Wednesday, Thursday, Friday, Saturday, Sunday
    ];
}
