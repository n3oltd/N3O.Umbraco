using N3O.Umbraco.Localization;
using NodaTime;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace N3O.Umbraco.Lookups;

public class DayOfMonth : NamedLookup, IComparable<DayOfMonth>, IEquatable<DayOfMonth>, IComparable<int>, IEquatable<int> {
    public DayOfMonth(int day) : base(day.ToString(), day.ToString()) {
        Day = day;
    }

    public LocalDate GetDateIn(int year, int month) {
        var day = Math.Min(Day, DateTime.DaysInMonth(year, month));

        return new LocalDate(year, month, day);
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

    public bool Equals(DayOfMonth other) {
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

        return obj.GetType() == GetType() && Equals((DayOfMonth) obj);
    }

    public int CompareTo(DayOfMonth other) {
        return CompareTo(other.Day);
    }

    public int CompareTo(int other) {
        return Day.CompareTo(other);
    }

    public override int GetHashCode() {
        return Day.GetHashCode();
    }
    
    private LocalDate GetNextDate(ILocalClock localClock, Func<LocalDate, LocalDate, bool> rollForwardPredicate) {
        var today = localClock.GetLocalToday();

        var date = GetDateIn(today.Year, today.Month);

        var rollForward = rollForwardPredicate(today, date);
        
        if (rollForward) {
            date = date.PlusMonths(1);
        }

        return date;
    }
}

public class DaysOfMonth : LookupsCollection<DayOfMonth> {
    public static readonly IReadOnlyList<DayOfMonth> All;

    static DaysOfMonth() {
        var list = new List<DayOfMonth>();

        for (var i = 1; i <= 31; i++) {
            var dayOfMonth = new DayOfMonth(i);

            list.Add(dayOfMonth);
        }

        All = list;
    }

    protected override Task<IReadOnlyList<DayOfMonth>> LoadAllAsync() {
        return Task.FromResult(All);
    }
}
