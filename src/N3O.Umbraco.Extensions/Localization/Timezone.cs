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
}

public class Timezones : LookupsCollection<Timezone> {
    private static readonly IReadOnlyList<Timezone> All;

    static Timezones() {
        All = TzdbDateTimeZoneSource.Default
                                    .ZoneLocations
                                    .Select(t => new Timezone(t.ZoneId.ToLowerInvariant(), t.ZoneId, SystemClock.Instance))
                                    .Concat(Utc)
                                    .OrderBy(t => t.UtcOffset)
                                    .ThenBy(t => t == Utc ? 0 : 1)
                                    .ThenBy(t => t.ToString())
                                    .ToList();
    }

    public static Timezone Utc => new(DateTimeZone.Utc.Id.ToLowerInvariant(),
                                      DateTimeZone.Utc.Id,
                                      SystemClock.Instance);

    protected override Task<IReadOnlyList<Timezone>> LoadAllAsync() {
        return Task.FromResult(All);
    }
}
