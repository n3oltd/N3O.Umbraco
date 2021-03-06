using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Localization;
using System;
using OurDataTypes = N3O.Umbraco.Data.Lookups.DataTypes;

namespace N3O.Umbraco.Data.Converters;

public class DecimalCellConverter :
    INullableCellConverter<decimal>,
    INullableCellConverter<float>,
    INullableCellConverter<double> {
    public Cell Convert(IFormatter formatter, ILocalClock clock, decimal value, Type targetType) {
        return Convert(formatter, clock, (decimal?) value, targetType);
    }

    public Cell Convert(IFormatter formatter, ILocalClock clock, decimal? value, Type targetType) {
        return OurDataTypes.Decimal.Cell(value);
    }

    public Cell Convert(IFormatter formatter, ILocalClock clock, float value, Type targetType) {
        return Convert(formatter, clock, (float?) value, targetType);
    }

    public Cell Convert(IFormatter formatter, ILocalClock clock, float? value, Type targetType) {
        return Convert(formatter, clock, (decimal?) value, targetType);
    }

    public Cell Convert(IFormatter formatter, ILocalClock clock, double value, Type targetType) {
        return Convert(formatter, clock, (double?) value, targetType);
    }

    public Cell Convert(IFormatter formatter, ILocalClock clock, double? value, Type targetType) {
        return Convert(formatter, clock, (decimal?) value, targetType);
    }
}
