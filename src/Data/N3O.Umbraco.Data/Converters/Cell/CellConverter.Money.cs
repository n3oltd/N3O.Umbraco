using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Localization;
using System;
using OurDataTypes = N3O.Umbraco.Data.Lookups.DataTypes;

namespace N3O.Umbraco.Data.Converters;

public class MoneyCellConverter : ICellConverter<Money> {
    public Cell Convert(IFormatter formatter, ILocalClock clock, Money value, Type targetType) {
        return OurDataTypes.Money.Cell(value);
    }
}
