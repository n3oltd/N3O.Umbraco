using N3O.Umbraco.Data.Builders;
using N3O.Umbraco.Data.Lookups;
using N3O.Umbraco.Data.Parsing;
using N3O.Umbraco.Localization;
using System.Collections.Generic;
using System.IO;

namespace N3O.Umbraco.Data;

public interface IWorkspace {
    ICsvWorkbook CreateCsvWorkbook(LocalizationSettings localizationSettings = null);
    IExcelWorkbook CreateExcelWorkbook();

    ICsvReader GetCsvReader(DatePattern datePattern,
                            DecimalSeparator decimalSeparator,
                            IEnumerable<IBlobResolver> blobResolvers,
                            TextEncoding textEncoding,
                            Stream stream,
                            bool hasColumnHeadings,
                            string delimiter = DataConstants.Delimiters.Comma);

    IColumnRangeBuilder ColumnRangeBuilder { get; }
    ITableBuilder TableBuilder { get; }
}
