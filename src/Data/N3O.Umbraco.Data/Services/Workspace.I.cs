using N3O.Umbraco.Data.Builders;
using N3O.Umbraco.Data.Lookups;
using System.IO;

namespace N3O.Umbraco.Data.Services {
    public interface IWorkspace {
        ICsvWorkbook CreateCsvWorkbook();
        IExcelWorkbook CreateExcelWorkbook();

        ICsvReader GetCsvReader(DatePattern datePattern,
                                DecimalSeparator decimalSeparator,
                                TextEncoding textEncoding,
                                Stream stream,
                                bool hasColumnHeadings);
    
        IColumnRangeBuilder ColumnRangeBuilder { get; }
        ITableBuilder TableBuilder { get; }
    }
}