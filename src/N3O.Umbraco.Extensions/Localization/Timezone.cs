using N3O.Umbraco.Extensions;
using N3O.Umbraco.Lookups;
using NodaTime;
using NodaTime.TimeZones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace N3O.Umbraco.Localization;

public class Timezone : NamedLookup {
    private readonly IClock _clock;

    public Timezone(string id, string name, IClock clock) : base(id, name) {
        _clock = clock;

        var zoneId = DateTimeZoneProviders.Tzdb.Ids.Single(x => x.EqualsInvariant(id));
        Zone = DateTimeZoneProviders.Tzdb[zoneId];
    }

    public DateTimeZone Zone { get; }

    public Offset UtcOffset => Zone.Id == DateTimeZone.Utc.Id ?
                                   Offset.FromSeconds(0) :
                                   Zone.GetUtcOffset(_clock.GetCurrentInstant());

    public override string ToString() {
        if (Zone == DateTimeZone.Utc) {
            return "(UTC) Coordinated Universal Time";
        }

        var offsetTimespan = TimeSpan.FromMilliseconds(Math.Abs(UtcOffset.Milliseconds));

        var offsetText = "UTC";

        if (UtcOffset.Milliseconds >= 0) {
            offsetText += "+";
        } else {
            offsetText += "-";
        }

        offsetText += $"{offsetTimespan.Hours:00}:{offsetTimespan.Minutes:00}";

        return $"({offsetText}) {Zone.Id}";
    }
    
    public static Timezone FromTzId(string tzId) {
        var tz = TzdbDateTimeZoneSource.Default.ZoneLocations.SingleOrDefault(x => x.ZoneId.EqualsInvariant(tzId));

        if (tz == null) {
            throw new Exception($"Unable to find timezone with ID {tzId}");
        }

        return ToTimezone(tz);
    }
    
    private static Timezone ToTimezone(TzdbZoneLocation zoneLocation) {
        return new Timezone(zoneLocation.ZoneId.ToLowerInvariant(), zoneLocation.ZoneId, SystemClock.Instance);
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

    protected override Task<IReadOnlyList<Timezone>> LoadAllAsync() {
        return Task.FromResult(All);
    }
}
