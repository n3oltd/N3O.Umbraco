using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Localization;
using System;

namespace N3O.Umbraco.Data.Converters {
    public class MoneyCellConverter : ICellConverter<Money> {
        public Cell Convert(IFormatter formatter, ILocalClock clock, Money value, Type targetType) {
            return DataTypes.Money.Cell(value);
        }
    }
}