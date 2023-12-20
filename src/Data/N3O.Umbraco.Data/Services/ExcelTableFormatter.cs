using N3O.Umbraco.Data.Converters;
using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Extensions;
using System.Collections.Generic;

namespace N3O.Umbraco.Data;

public class ExcelTableFormatter : IExcelTableFormatter {
    private readonly IExcelCellFormatter _excelCellFormatter;
    private readonly IReadOnlyDictionary<Column, IExcelCellConverter> _customColumnConverters;
    private readonly IReadOnlyDictionary<Column, AggregationFunction> _footerFunctions;

    public ExcelTableFormatter(IExcelCellFormatter excelCellFormatter,
                               IReadOnlyDictionary<Column, IExcelCellConverter> customColumnConverters,
                               IReadOnlyDictionary<Column, AggregationFunction> footerFunctions) {
        _excelCellFormatter = excelCellFormatter;
        _customColumnConverters = customColumnConverters;
        _footerFunctions = footerFunctions;
    }

    public ExcelCell FormatCell(Column column, Cell cell) {
        ExcelCell excelCell = null;

        if (cell != null) {
            var customConverter = _customColumnConverters.GetValueOrDefault(column);

            if (customConverter.HasValue()) {
                excelCell = customConverter.GetExcelCell(cell, column.Formatter, column.Comment);
            } else {
                excelCell = _excelCellFormatter.FormatCell(cell, column.Formatter);
            }
        }
        
        return excelCell;
    }

    public ExcelColumn FormatColumn(Column column) {
        var footerFunction = _footerFunctions.GetValueOrDefault(column);

        var excelColumn = new ExcelColumn(column, footerFunction);

        return excelColumn;
    }
}
