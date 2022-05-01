using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Localization;
using NodaTime;
using System;
using OurDataTypes = N3O.Umbraco.Data.Lookups.DataTypes;

namespace N3O.Umbraco.Data.Converters {
    public class DateCellConverter :
        INullableCellConverter<Instant>,
        INullableCellConverter<LocalDateTime>,
        INullableCellConverter<LocalDate> {
        public Cell Convert(IFormatter formatter, ILocalClock clock, Instant value, Type targetType) {
            return Convert(formatter, clock, (Instant?) value, targetType);
        }

        public Cell Convert(IFormatter formatter, ILocalClock clock, Instant? value, Type targetType) {
            LocalDate? date = null;

            if (value != null) {
                date = value.Value.InZone(clock.GetTimezone().Zone).Date;
            }

            var cell = GetCell(date);

            return cell;
        }

        public Cell Convert(IFormatter formatter, ILocalClock clock, LocalDateTime value, Type targetType) {
            return Convert(formatter, clock, (LocalDateTime?) value, targetType);
        }

        public Cell Convert(IFormatter formatter, ILocalClock clock, LocalDateTime? value, Type targetType) {
            return GetCell(value?.Date);
        }

        public Cell Convert(IFormatter formatter, ILocalClock clock, LocalDate value, Type targetType) {
            return Convert(formatter, clock, (LocalDate?) value, targetType);
        }

        public Cell Convert(IFormatter formatter, ILocalClock clock, LocalDate? value, Type targetType) {
            return GetCell(value);
        }

        private Cell GetCell(LocalDate? value) {
            return OurDataTypes.Date.Cell(value);
        }
    }
}