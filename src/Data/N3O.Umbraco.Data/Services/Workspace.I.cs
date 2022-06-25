using N3O.Umbraco.Data.Builders;
using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Data.Parsing;
using System.Collections.Generic;
using System.IO;

namespace N3O.Umbraco.Data;

public interface IWorkspace {
    ICsvWorkbook CreateCsvWorkbook();
    IExcelWorkbook CreateExcelWorkbook();

    ICsvReader GetCsvReader(DatePattern datePattern,
                            DecimalSeparator decimalSeparator,
                            IEnumerable<IBlobResolver> blobResolvers,
                            TextEncoding textEncoding,
                            Stream stream,
                            bool hasColumnHeadings);

    IColumnRangeBuilder ColumnRangeBuilder { get; }
    ITableBuilder TableBuilder { get; }
}
