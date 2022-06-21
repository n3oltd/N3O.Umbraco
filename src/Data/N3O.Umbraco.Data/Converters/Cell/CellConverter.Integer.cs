using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Localization;
using System;
using OurDataTypes = N3O.Umbraco.Data.Lookups.DataTypes;

namespace N3O.Umbraco.Data.Converters;

public class IntegerCellConverter :
    INullableCellConverter<long>,
    INullableCellConverter<int> {
    public Cell Convert(IFormatter formatter, ILocalClock clock, long value, Type targetType) {
        return Convert(formatter, clock, (long?) value, targetType);
    }

    public Cell Convert(IFormatter formatter, ILocalClock clock, long? value, Type targetType) {
        return OurDataTypes.Integer.Cell(value);
    }

    public Cell Convert(IFormatter formatter, ILocalClock clock, int value, Type targetType) {
        return Convert(formatter, clock, (int?) value, targetType);
    }

    public Cell Convert(IFormatter formatter, ILocalClock clock, int? value, Type targetType) {
        return Convert(formatter, clock, (long?) value, targetType);
    }
}
