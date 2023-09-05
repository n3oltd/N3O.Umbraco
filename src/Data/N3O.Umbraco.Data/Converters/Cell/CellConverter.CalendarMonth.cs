using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Localization;
using System;
using OurDataTypes = N3O.Umbraco.Data.Lookups.DataTypes;

namespace N3O.Umbraco.Data.Converters;

public class CalendarMonthCellConverter : ICellConverter<CalendarMonth> {
    public Cell Convert(IFormatter formatter, ILocalClock clock, CalendarMonth value, Type targetType) {
        return OurDataTypes.CalendarMonth.Cell(value, targetType);
    }
}
