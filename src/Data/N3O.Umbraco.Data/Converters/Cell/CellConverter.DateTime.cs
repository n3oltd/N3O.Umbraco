using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Localization;
using NodaTime;
using System;

namespace N3O.Umbraco.Data.Converters {
    public class DateTimeCellConverter :
        INullableCellConverter<Instant>,
        INullableCellConverter<LocalDateTime>,
        INullableCellConverter<ZonedDateTime> {
        public Cell Convert(IFormatter formatter, ILocalClock clock, Instant value, Type targetType) {
            return Convert(formatter, clock, (Instant?) value, targetType);
        }

        public Cell Convert(IFormatter formatter, ILocalClock clock, Instant? value, Type targetType) {
            LocalDateTime? dateTime = null;

            if (value != null) {
                dateTime = value.Value.InZone(clock.GetTimezone().Zone).LocalDateTime;
            }

            var cell = GetCell(dateTime);

            return cell;
        }

        public Cell Convert(IFormatter formatter, ILocalClock clock, LocalDateTime value, Type targetType) {
            return Convert(formatter, clock, (LocalDateTime?) value, targetType);
        }

        public Cell Convert(IFormatter formatter, ILocalClock clock, LocalDateTime? value, Type targetType) {
            return GetCell(value);
        }

        public Cell Convert(IFormatter formatter, ILocalClock clock, ZonedDateTime value, Type targetType) {
            return Convert(formatter, clock, (ZonedDateTime?) value, targetType);
        }

        public Cell Convert(IFormatter formatter, ILocalClock clock, ZonedDateTime? value, Type targetType) {
            return GetCell(value?.LocalDateTime);
        }

        private Cell GetCell(LocalDateTime? value) {
            return DataTypes.DateTime.Cell(value);
        }
    }
}