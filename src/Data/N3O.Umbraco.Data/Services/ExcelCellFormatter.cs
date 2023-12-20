using N3O.Umbraco.Data.Converters;
using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Localization;
using System.Collections.Generic;
using System.Linq;
using OurDataTypes = N3O.Umbraco.Data.Lookups.DataTypes;

namespace N3O.Umbraco.Data;

public class ExcelCellFormatter : IExcelCellFormatter {
    private readonly IReadOnlyDictionary<DataType, IDefaultExcelCellConverter> _defaultCellConverters;

    public ExcelCellFormatter(IEnumerable<IDefaultExcelCellConverter> cellConverters) {
        _defaultCellConverters = MapDefaultCellConverters(cellConverters.ToList());
    }

    private IReadOnlyDictionary<DataType, IDefaultExcelCellConverter> MapDefaultCellConverters(IReadOnlyList<IDefaultExcelCellConverter> cellConverters) {
        var dict = new Dictionary<DataType, IDefaultExcelCellConverter>();

        foreach (var dataType in OurDataTypes.GetAllTypes()) {
            var converterType = typeof(ExcelCellConverter<>).MakeGenericType(dataType.GetClrType());
            var converter = cellConverters.Single(x => converterType.IsInstanceOfType(x));

            dict[dataType] = converter;
        }

        return dict;
    }

    public ExcelCell FormatCell(Cell cell, IFormatter formatter) {
        ExcelCell excelCell = null;
        
        if (cell != null) {
            var cellConverter = _defaultCellConverters[cell.Type];

            excelCell = cellConverter.GetExcelCell(cell, formatter);
        }

        return excelCell;
    }
}
