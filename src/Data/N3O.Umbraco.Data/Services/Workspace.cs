using N3O.Umbraco.Data.Builders;
using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Data.Parsing;
using System.Collections.Generic;
using System.IO;

namespace N3O.Umbraco.Data;

public class Workspace : IWorkspace {
    private readonly IParserFactory _parserFactory;
    private readonly IColumnRangeBuilder _columnRangeBuilder;

    public Workspace(ITableBuilder tableBuilder,
                     IColumnRangeBuilder columnRangeBuilder,
                     IDataSummaryBuilder dataSummaryBuilder,
                     IParserFactory parserFactory) {
        ColumnRangeBuilder = columnRangeBuilder;
        DataSummaryBuilder = dataSummaryBuilder;
        TableBuilder = tableBuilder;
        _columnRangeBuilder = columnRangeBuilder;
        _parserFactory = parserFactory;
        
    }

    public ICsvWorkbook CreateCsvWorkbook() {
        var workbook = new CsvWorkbook();

        return workbook;
    }

    public IExcelWorkbook CreateExcelWorkbook() {
        var workbook = new ExcelWorkbook();

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
    public IDataSummaryBuilder DataSummaryBuilder { get; }
    public ITableBuilder TableBuilder { get; }
}
