using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Lookups;
using System;

namespace N3O.Umbraco.Data.Converters {
    public class LookupCellConverter : ICellConverter<INamedLookup> {
        public Cell Convert(IFormatter formatter, ILocalClock clock, INamedLookup value, Type targetType) {
            return DataTypes.Lookup.Cell(value, targetType);
        }
    }
}