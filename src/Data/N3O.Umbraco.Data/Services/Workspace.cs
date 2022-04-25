using N3O.Umbraco.Data.Builders;
using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Data.Parsing;
using System.Collections.Generic;
using System.IO;

namespace N3O.Umbraco.Data.Services {
    public class Workspace : IWorkspace {
        private readonly IParserFactory _parserFactory;
        private readonly IColumnVisibility _columnVisibility;
        private readonly IColumnRangeBuilder _columnRangeBuilder;

        public Workspace(ITableBuilder tableBuilder,
                         IColumnRangeBuilder columnRangeBuilder,
                         IParserFactory parserFactory,
                         IColumnVisibility columnVisibility) {
            TableBuilder = tableBuilder;
            ColumnRangeBuilder = columnRangeBuilder;
            _columnRangeBuilder = columnRangeBuilder;
            _parserFactory = parserFactory;
            _columnVisibility = columnVisibility;
        }

        public ICsvWorkbook CreateCsvWorkbook() {
            var workbook = new CsvWorkbook(_columnRangeBuilder, _columnVisibility);

            return workbook;
        }

        public IExcelWorkbook CreateExcelWorkbook() {
            var workbook = new ExcelWorkbook(_columnVisibility);

            return workbook;
        }

        public ICsvReader GetCsvReader(DatePattern datePattern,
                                       DecimalSeparator decimalSeparator,
                                       IEnumerable<IBlobResolver> blobResolvers,
                                       TextEncoding textEncoding,
                                       Stream stream,
                                       bool hasColumnHeadings) {
            var parser = _parserFactory.GetParser(datePattern, decimalSeparator, blobResolvers);
            
            return new CsvReader(parser,
                                 TableBuilder,
                                 _columnRangeBuilder,
                                 textEncoding,
                                 stream,
                                 hasColumnHeadings);
        }

        public IColumnRangeBuilder ColumnRangeBuilder { get; }
        public ITableBuilder TableBuilder { get; }
    }

}