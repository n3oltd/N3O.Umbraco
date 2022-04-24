using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Localization;
using System;

namespace N3O.Umbraco.Data.Converters {
    public class StringCellConverter : ICellConverter<string> {
        public Cell Convert(IFormatter formatter, ILocalClock clock, string value, Type targetType) {
            return DataTypes.String.Cell(value);
        }
    }
}