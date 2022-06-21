using Microsoft.Extensions.DependencyInjection;
using N3O.Umbraco.Data.Converters;
using N3O.Umbraco.Data.Extensions;
using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Data.Services;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Lookups;
using System;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Data.Builders;

public class FluentExcelTableBuilder : IFluentExcelTableBuilder {
    private readonly IServiceProvider _serviceProvider;
    private readonly ILookups _lookups;
    private readonly IReadOnlyList<IExcelCellConverter> _defaultConverters;
    private readonly ITable _table;
    private readonly Dictionary<Column, IExcelCellConverter> _customColumnConverters = new();
    private readonly Dictionary<Column, AggregationFunction> _footerFunctions = new();

    public FluentExcelTableBuilder(IServiceProvider serviceProvider,
                                   ILookups lookups,
                                   IEnumerable<IDefaultExcelCellConverter> defaultConverters,
                                   ITable table) {
        _serviceProvider = serviceProvider;
        _lookups = lookups;
        _defaultConverters = defaultConverters.ToList();
        _table = table;

        PopulateFooterFunctions();
    }

    public void FormatColumn<TExcelCellConverter>(Column column) {
        var converter = GetExcelCellConverter(typeof(TExcelCellConverter));

        _customColumnConverters[column] = converter;
    }

    public void Footer(Column column, AggregationFunction footerFunction) {
        _footerFunctions[column] = footerFunction;
    }

    public IExcelTable Build() {
        var tableFormatter = new ExcelTableFormatter(_defaultConverters,
                                                     _customColumnConverters,
                                                     _footerFunctions);

        return new ExcelTable(_table, tableFormatter);
    }

    public IExcelCellConverter GetExcelCellConverter(Type cellConverterType) {
        if (!cellConverterType.ImplementsInterface<IExcelCellConverter>()) {
            throw new Exception($"The specified Excel cell converter type {cellConverterType.FullName.Quote()} does not implement {nameof(IExcelCellConverter)}");
        }

        var excelCellConverter = (IExcelCellConverter) _serviceProvider.GetRequiredService(cellConverterType);

        return excelCellConverter;
    }

    private void PopulateFooterFunctions() {
        var footerFunctions = _lookups.GetAll<AggregationFunction>();

        foreach (var column in _table.Columns) {
            var footerFunction = footerFunctions.SingleOrDefault(x => x.Applies(column));

            _footerFunctions[column] = footerFunction;
        }
    }
}
