using NodaTime;
using NodaTime.TimeZones;

namespace N3O.Umbraco.Localization;

public class LocalClock : ILocalClock {
    private readonly ILocalizationSettingsAccessor _settingsAccessor;
    private readonly IClock _clock;

    public LocalClock(ILocalizationSettingsAccessor settingsAccessor, IClock clock) {
        _settingsAccessor = settingsAccessor;
        _clock = clock;
    }

    public Instant GetCurrentInstant() {
        return _clock.GetCurrentInstant();
    }

    public LocalDate GetLocalToday() {
        var now = GetLocalNow();

        return now.Date;
    }

    public LocalDateTime GetLocalNow() {
        var zonedDateTime = GetZonedNow();

        return zonedDateTime.LocalDateTime;
    }

    public DateTimeZone GetZone() {
        var timezone = GetTimezone();

        return timezone.Zone;
    }

    public Timezone GetTimezone() {
        var timezone = _settingsAccessor.GetSettings()
                                        .Timezone;

        return timezone;
    }

    public ZonedDateTime GetZonedNow() {
        var timezone = GetTimezone();
        var instant = GetCurrentInstant();

        return instant.InZone(timezone.Zone);
    }

    public Instant ToInstant(LocalDateTime localDateTime) {
        var zonedDateTime = ToZonedDateTime(localDateTime);

        return zonedDateTime.ToInstant();
    }

    public LocalDateTime ToLocalDateTime(Instant instant) {
        var zonedDateTime = ToZonedDateTime(instant);

        return zonedDateTime.LocalDateTime;
    }

    public LocalDateTime ToLocalDateTime(LocalDateTime localDateTime) {
        var zonedDateTime = ToZonedDateTime(localDateTime);

        return zonedDateTime.LocalDateTime;
    }

    public ZonedDateTime ToZonedDateTime(Instant instant) {
        var dateTimeZone = GetZone();

        return instant.InZone(dateTimeZone);
    }

    public ZonedDateTime ToZonedDateTime(LocalDateTime localDateTime) {
        var dateTimeZone = GetZone();

        return localDateTime.InZone(dateTimeZone, Resolvers.LenientResolver);
    }
}