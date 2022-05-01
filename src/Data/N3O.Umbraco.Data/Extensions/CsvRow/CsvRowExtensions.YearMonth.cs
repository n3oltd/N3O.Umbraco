using N3O.Umbraco.Data.Models;
using NodaTime;
using OurDataTypes = N3O.Umbraco.Data.Lookups.DataTypes;

namespace N3O.Umbraco.Data.Extensions {
    public static partial class CsvRowExtensions {
        public static YearMonth? GetYearMonth(this CsvRow csvRow, string heading) {
            return GetYearMonth(csvRow, CsvSelect.For(heading));
        }

        public static YearMonth? GetYearMonth(this CsvRow csvRow, int index) {
            return GetYearMonth(csvRow, CsvSelect.For(index));
        }

        public static YearMonth? GetYearMonth(this CsvRow csvRow, CsvSelect select) {
            return csvRow.ParseField(select,
                                     (parser, field) => parser.YearMonth.Parse(field,
                                                                               OurDataTypes.YearMonth.GetClrType()));
        }
    }
}