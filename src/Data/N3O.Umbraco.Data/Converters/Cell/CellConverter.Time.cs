using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Localization;
using NodaTime;
using System;
using OurDataTypes = N3O.Umbraco.Data.Lookups.DataTypes;

namespace N3O.Umbraco.Data.Converters {
    public class TimeCellConverter :
        INullableCellConverter<Instant>,
        INullableCellConverter<LocalTime>,
        INullableCellConverter<LocalDateTime>,
        INullableCellConverter<ZonedDateTime> {
        public Cell Convert(IFormatter formatter, ILocalClock clock, Instant value, Type targetType) {
            return Convert(formatter, clock, (Instant?) value, targetType);
        }

        public Cell Convert(IFormatter formatter, ILocalClock clock, Instant? value, Type targetType) {
            LocalTime? time = null;

            if (value != null) {
                time = value.Value.InZone(clock.GetTimezone().Zone).TimeOfDay;
            }

            var cell = GetCell(time);

            return cell;
        }

        public Cell Convert(IFormatter formatter, ILocalClock clock, LocalTime value, Type targetType) {
            return Convert(formatter, clock, (LocalTime?) value, targetType);
        }

        public Cell Convert(IFormatter formatter, ILocalClock clock, LocalTime? value, Type targetType) {
            return GetCell(value);
        }

        public Cell Convert(IFormatter formatter, ILocalClock clock, LocalDateTime value, Type targetType) {
            return Convert(formatter, clock, (LocalDateTime?) value, targetType);
        }

        public Cell Convert(IFormatter formatter, ILocalClock clock, LocalDateTime? value, Type targetType) {
            LocalTime? time = value?.TimeOfDay;

            return GetCell(time);
        }

        public Cell Convert(IFormatter formatter, ILocalClock clock, ZonedDateTime value, Type targetType) {
            return Convert(formatter, clock, (ZonedDateTime?) value, targetType);
        }

        public Cell Convert(IFormatter formatter, ILocalClock clock, ZonedDateTime? value, Type targetType) {
            return GetCell(value?.LocalDateTime.TimeOfDay);
        }

        private Cell GetCell(LocalTime? value) {
            var cell = OurDataTypes.Time.Cell(value);

            return cell;
        }
    }
}