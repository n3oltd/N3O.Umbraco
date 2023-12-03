using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Localization;
using Umbraco.Extensions;

namespace N3O.Umbraco.Data.Extensions;

public static class CellExtensions {
    public static ExcelNumberFormat GetExcelNumberFormat(this Cell cell, IFormatter formatter) {
        if (cell.Type == Lookups.DataTypes.Blob) {
            return new StringExcelNumberFormat();
        } else if (cell.Type == Lookups.DataTypes.Bool) {
            return new BoolExcelNumberFormat();
        } else if (cell.Type == Lookups.DataTypes.CalendarMonth) {
            return new CalendarMonthExcelNumberFormat();
        } else if (cell.Type == Lookups.DataTypes.CalendarWeek) {
            return new CalendarWeekExcelNumberFormat();
        } else if (cell.Type == Lookups.DataTypes.Content) {
            return new StringExcelNumberFormat();
        } else if (cell.Type == Lookups.DataTypes.Date) {
            return new DateExcelNumberFormat(formatter.DateTime.DateFormat);
        } else if (cell.Type == Lookups.DataTypes.DateTime) {
            return new DateTimeExcelNumberFormat(formatter.DateTime.DateFormat,
                                                 formatter.DateTime.TimeFormat);
        } else if (cell.Type == Lookups.DataTypes.Decimal) {
            return new DecimalExcelNumberFormat();
        } else if (cell.Type == Lookups.DataTypes.Guid) {
            return new StringExcelNumberFormat();
        } else if (cell.Type == Lookups.DataTypes.Integer) {
            return new IntegerExcelNumberFormat();
        } else if (cell.Type == Lookups.DataTypes.Lookup) {
            return new LookupExcelNumberFormat();
        } else if (cell.Type == Lookups.DataTypes.Money) {
            return new MoneyExcelNumberFormat(cell.GetValue<Money>()?.Currency);
        } else if (cell.Type == Lookups.DataTypes.PublishedContent) {
            return new StringExcelNumberFormat();
        } else if (cell.Type == Lookups.DataTypes.Reference) {
            return new ReferenceExcelNumberFormat();
        } else if (cell.Type == Lookups.DataTypes.String) {
            return new StringExcelNumberFormat();
        } else if (cell.Type == Lookups.DataTypes.Time) {
            return new TimeExcelNumberFormat(formatter.DateTime.TimeFormat);
        } else {
            throw UnrecognisedValueException.For(cell.Type);
        }
    }

    public static T GetValue<T>(this Cell cell) {
        return (T) cell.Value;
    }

    public static void SetTitleMetadata(this Cell cell, string title) {
        cell.Metadata[DataConstants.MetadataKeys.Cells.Title] = title.Yield();
    }
}
