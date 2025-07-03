using N3O.Umbraco.Extensions;
using N3O.Umbraco.Lookups;
using NodaTime;
using NodaTime.TimeZones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Localization;

public class Timezone : NamedLookup {
    public Timezone(IClock clock, DateTimeZone zone) : base(zone.Id, GetName(clock, zone)) {
        UtcOffset = GetUtcOffset(clock, zone);
        Zone = zone;
    }

    public Offset UtcOffset { get; }
    public DateTimeZone Zone { get; }
    
    public static Timezone FromTzId(string tzId) {
        var dateTimeZone = DateTimeZoneProviders.Tzdb.GetZoneOrNull(tzId);

        return ToTimezone(dateTimeZone);
    }
    
    private static string GetName(IClock clock, DateTimeZone dateTimeZone) {
        if (dateTimeZone == DateTimeZone.Utc) {
            return "(UTC) Coordinated Universal Time";
        }

        var utcOffset = GetUtcOffset(clock, dateTimeZone);
        var offsetTimespan = TimeSpan.FromMilliseconds(Math.Abs(utcOffset.Milliseconds));
        var offsetText = "UTC";

        if (utcOffset.Milliseconds >= 0) {
            offsetText += "+";
        } else {
            offsetText += "-";
        }

        offsetText += $"{offsetTimespan.Hours:00}:{offsetTimespan.Minutes:00}";

        return $"({offsetText}) {dateTimeZone.Id}";
    }

    private static Offset GetUtcOffset(IClock clock, DateTimeZone dateTimeZone) {
        return dateTimeZone == DateTimeZone.Utc
                   ? Offset.FromSeconds(0)
                   : dateTimeZone.GetUtcOffset(clock.GetCurrentInstant());
    }
    
    private static Timezone ToTimezone(DateTimeZone dateTimeZone) {
        return new Timezone(SystemClock.Instance, dateTimeZone);
    }
}

public class Timezones : LookupsCollection<Timezone> {
    private static readonly IReadOnlyList<Timezone> All;

    static Timezones() {
        All = TzdbDateTimeZoneSource.Default
                                    .ZoneLocations
                                    .Select(x => Timezone.FromTzId(x.ZoneId))
                                    .Concat(Utc)
                                    .OrderBy(t => t.UtcOffset)
                                    .ThenBy(t => t == Utc ? 0 : 1)
                                    .ThenBy(t => t.ToString())
                                    .ToList();
    }

    public static Timezone Utc => Timezone.FromTzId("Etc/UTC");

    protected override Task<IReadOnlyList<Timezone>> LoadAllAsync(CancellationToken cancellationToken) {
        return Task.FromResult(All);
    }
}
