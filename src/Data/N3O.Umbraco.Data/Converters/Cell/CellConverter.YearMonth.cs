using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Localization;
using NodaTime;
using System;
using OurDataTypes = N3O.Umbraco.Data.Lookups.DataTypes;

namespace N3O.Umbraco.Data.Converters {
    public class YearMonthCellConverter : INullableCellConverter<YearMonth> {
        public Cell Convert(IFormatter formatter, ILocalClock clock, YearMonth value, Type targetType) {
            return Convert(formatter, clock, (YearMonth?) value, targetType);
        }
        
        public Cell Convert(IFormatter formatter, ILocalClock clock, YearMonth? value, Type targetType) {
            return OurDataTypes.YearMonth.Cell(value);
        }
    }
}