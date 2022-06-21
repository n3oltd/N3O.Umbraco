using N3O.Umbraco.Data.Converters;
using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Data.Models;
using System.Collections.Generic;
using System.Linq;
using OurDataTypes = N3O.Umbraco.Data.Lookups.DataTypes;

namespace N3O.Umbraco.Data.Services;

public class ExcelTableFormatter : IExcelTableFormatter {
    private readonly IReadOnlyDictionary<DataType, IExcelCellConverter> _defaultConverters;
    private readonly IReadOnlyDictionary<Column, IExcelCellConverter> _customColumnConverters;
    private readonly IReadOnlyDictionary<Column, AggregationFunction> _footerFunctions;

    public ExcelTableFormatter(IEnumerable<IExcelCellConverter> defaultConverters,
                               IReadOnlyDictionary<Column, IExcelCellConverter> customColumnConverters,
                               IReadOnlyDictionary<Column, AggregationFunction> footerFunctions) {
        _defaultConverters = MapDefaultConverters(defaultConverters.ToList());
        _customColumnConverters = customColumnConverters;
        _footerFunctions = footerFunctions;
    }

    private IReadOnlyDictionary<DataType, IExcelCellConverter> MapDefaultConverters(IReadOnlyList<IExcelCellConverter> defaultConverters) {
        var dict = new Dictionary<DataType, IExcelCellConverter>();

        foreach (var dataType in OurDataTypes.GetAllTypes()) {
            var converterType = typeof(ExcelCellConverter<>).MakeGenericType(dataType.GetClrType());
            var converter = defaultConverters.Single(x => converterType.IsInstanceOfType(x));

            dict[dataType] = converter;
        }

        return dict;
    }

    public ExcelCell FormatCell(Column column, Cell cell) {
        return FormatCell(_customColumnConverters, column, cell);
    }

    public ExcelColumn FormatColumn(Column column) {
        var footerFunction = _footerFunctions.GetValueOrDefault(column);

        var excelColumn = new ExcelColumn(column, footerFunction);

        return excelColumn;
    }

    private ExcelCell FormatCell(IReadOnlyDictionary<Column, IExcelCellConverter> converters,
                                  Column column,
                                  Cell cell) {
        ExcelCell excelCell = null;
        
        if (cell != null) {
            var defaultConverter = _defaultConverters[cell.Type];
            var converter = converters.GetValueOrDefault(column, defaultConverter);

            excelCell = converter.GetExcelCell(column, cell);
        }

        return excelCell;
    }
}
