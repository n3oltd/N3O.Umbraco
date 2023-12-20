using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Lookups;
using System;

namespace N3O.Umbraco.Data.Builders;

public class ExcelTableBuilder : IExcelTableBuilder {
    private readonly IServiceProvider _serviceProvider;
    private readonly ILookups _lookups;
    private readonly IExcelCellFormatter _excelCellFormatter;

    public ExcelTableBuilder(IServiceProvider serviceProvider,
                             ILookups lookups,
                             IExcelCellFormatter excelCellFormatter) {
        _serviceProvider = serviceProvider;
        _lookups = lookups;
        _excelCellFormatter = excelCellFormatter;
    }

    public IFluentExcelTableBuilder ForTable(ITable table) {
        return new FluentExcelTableBuilder(_serviceProvider, _lookups, _excelCellFormatter, table);
    }
}
