using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Localization;
using System;
using OurDataTypes = N3O.Umbraco.Data.Lookups.DataTypes;

namespace N3O.Umbraco.Data.Converters {
    public class GuidCellConverter : INullableCellConverter<Guid> {
        public Cell Convert(IFormatter formatter, ILocalClock clock, Guid value, Type targetType) {
            return Convert(formatter, clock, (Guid?) value, targetType);
        }

        public Cell Convert(IFormatter formatter, ILocalClock clock, Guid? value, Type targetType) {
            return OurDataTypes.Guid.Cell(value);
        }
    }
}