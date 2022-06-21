using N3O.Umbraco.Data.Models;
using NodaTime;
using OurDataTypes = N3O.Umbraco.Data.Lookups.DataTypes;

namespace N3O.Umbraco.Data.Extensions;

public static partial class CsvRowExtensions {
    public static LocalTime? GetTime(this CsvRow csvRow, string heading) {
        return GetTime(csvRow, CsvSelect.For(heading));
    }

    public static LocalTime? GetTime(this CsvRow csvRow, int index) {
        return GetTime(csvRow, CsvSelect.For(index));
    }

    public static LocalTime? GetTime(this CsvRow csvRow, CsvSelect select) {
        return csvRow.ParseField(select,
                                 (parser, field) => parser.Time.Parse(field, OurDataTypes.Time.GetClrType()));
    }
}
