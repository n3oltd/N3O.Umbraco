using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Localization;
using System;

namespace N3O.Umbraco.Data.Converters {
    public class BoolCellConverter : INullableCellConverter<bool> {
        public Cell Convert(IFormatter formatter, ILocalClock clock, bool value, Type targetType) {
            return Convert(formatter, clock, (bool?) value, targetType);
        }

        public Cell Convert(IFormatter formatter, ILocalClock clock, bool? value, Type targetType) {
            return DataTypes.Bool.Cell(value);
        }
    }
}