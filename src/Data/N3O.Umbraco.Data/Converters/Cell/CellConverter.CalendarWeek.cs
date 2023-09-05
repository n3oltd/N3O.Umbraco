using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Localization;
using System;
using OurDataTypes = N3O.Umbraco.Data.Lookups.DataTypes;

namespace N3O.Umbraco.Data.Converters;

public class CalendarWeekCellConverter : ICellConverter<CalendarWeek> {
    public Cell Convert(IFormatter formatter, ILocalClock clock, CalendarWeek value, Type targetType) {
        return OurDataTypes.CalendarWeek.Cell(value, targetType);
    }
}