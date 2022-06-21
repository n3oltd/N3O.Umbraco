using NodaTime;

namespace N3O.Umbraco.Localization;

public interface ILocalClock : IClock {
    LocalDate GetLocalToday();
    LocalDateTime GetLocalNow();
    Timezone GetTimezone();
    DateTimeZone GetZone();
    ZonedDateTime GetZonedNow();
    Instant ToInstant(LocalDateTime localDateTime);
    LocalDateTime ToLocalDateTime(Instant instant);
    LocalDateTime ToLocalDateTime(LocalDateTime localDateTime);
    ZonedDateTime ToZonedDateTime(Instant instant);
    ZonedDateTime ToZonedDateTime(LocalDateTime localDateTime);
}
