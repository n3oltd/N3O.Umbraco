using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Localization;
using System;

namespace N3O.Umbraco.Data.Converters;

public abstract class ExcelCellConverter<T> : IExcelCellConverter<T> {
    public ExcelCell<T> GetExcelCell(Cell<T> cell, IFormatter formatter, string comment = null) {
        var excelValue = GetExcelValue(cell.Value, formatter);
        var hyperLink = GetHyperlink(cell.Value);
        var formatting = GetFormatting(cell, formatter);

        var excelCell = ExcelCell.FromCell(cell, excelValue, comment, hyperLink, formatting);

        return excelCell;
    }

    public ExcelCell GetExcelCell(Cell cell, IFormatter formatter, string comment = null) {
        return GetExcelCell((Cell<T>) cell, formatter);
    }

    private ExcelFormatting GetFormatting(Cell<T> cell, IFormatter formatter) {
        var excelFormatting = new ExcelFormatting();

        var numberFormat = GetNumberFormat(cell.Value, formatter);
        if (numberFormat != null) {
            excelFormatting.NumberFormat = numberFormat;
        }

        ApplyFormatting(cell, excelFormatting, formatter);

        return excelFormatting;
    }

    protected virtual object GetExcelValue(T value, IFormatter formatter) {
        return value;
    }

    protected virtual Uri GetHyperlink(T value) {
        return null;
    }

    protected virtual ExcelNumberFormat GetNumberFormat(T value, IFormatter formatter) {
        return null;
    }

    protected virtual void ApplyFormatting(Cell<T> cell, ExcelFormatting formatting, IFormatter formatter) { }
}
