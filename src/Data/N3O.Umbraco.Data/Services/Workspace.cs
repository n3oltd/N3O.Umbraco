using N3O.Umbraco.Data.Builders;
using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Data.Parsing;
using N3O.Umbraco.Localization;
using System.Collections.Generic;
using System.IO;

namespace N3O.Umbraco.Data;

public class Workspace : IWorkspace {
    private readonly IParserFactory _parserFactory;
    private readonly IExcelCellFormatter _excelCellFormatter;
    private readonly IFormatter _formatter;
    private readonly IColumnRangeBuilder _columnRangeBuilder;

    public Workspace(ITableBuilder tableBuilder,
                     IColumnRangeBuilder columnRangeBuilder,
                     ISummaryFieldsBuilder summaryFieldsBuilder,
                     IParserFactory parserFactory,
                     IExcelCellFormatter excelCellFormatter,
                     IFormatter formatter) {
        ColumnRangeBuilder = columnRangeBuilder;
        SummaryFieldsBuilder = summaryFieldsBuilder;
        TableBuilder = tableBuilder;
        _columnRangeBuilder = columnRangeBuilder;
        _parserFactory = parserFactory;
        _excelCellFormatter = excelCellFormatter;
        _formatter = formatter;
    }

    public ICsvWorkbook CreateCsvWorkbook() {
        var workbook = new CsvWorkbook();

        return workbook;
    }

    public IExcelWorkbook CreateExcelWorkbook() {
        var workbook = new ExcelWorkbook(_excelCellFormatter, _formatter);

        return workbook;
    }

    public ICsvReader GetCsvReader(DatePattern datePattern,
                                   DecimalSeparator decimalSeparator,
                                   IEnumerable<IBlobResolver> blobResolvers,
                                   TextEncoding textEncoding,
                                   Stream stream,
                                   bool hasColumnHeadings,
                                   string delimiter = DataConstants.Delimiters.Comma) {
        var parser = _parserFactory.GetParser(datePattern, decimalSeparator, blobResolvers);
        
        return new CsvReader(parser,
                             TableBuilder,
                             _columnRangeBuilder,
                             textEncoding,
                             stream,
                             hasColumnHeadings,
                             delimiter);
    }

    public IColumnRangeBuilder ColumnRangeBuilder { get; }
    public ISummaryFieldsBuilder SummaryFieldsBuilder { get; }
    public ITableBuilder TableBuilder { get; }
}
