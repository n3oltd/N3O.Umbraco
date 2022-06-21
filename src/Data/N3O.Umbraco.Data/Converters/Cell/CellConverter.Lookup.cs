using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Lookups;
using System;
using OurDataTypes = N3O.Umbraco.Data.Lookups.DataTypes;

namespace N3O.Umbraco.Data.Converters;

public class LookupCellConverter : ICellConverter<INamedLookup> {
    public Cell Convert(IFormatter formatter, ILocalClock clock, INamedLookup value, Type targetType) {
        return OurDataTypes.Lookup.Cell(value, targetType);
    }
}
