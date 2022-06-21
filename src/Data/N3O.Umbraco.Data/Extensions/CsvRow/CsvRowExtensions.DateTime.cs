using N3O.Umbraco.Data.Models;
using NodaTime;
using OurDataTypes = N3O.Umbraco.Data.Lookups.DataTypes;

namespace N3O.Umbraco.Data.Extensions;

public static partial class CsvRowExtensions {
    public static LocalDateTime? GetDateTime(this CsvRow csvRow, string heading) {
        return GetDateTime(csvRow, CsvSelect.For(heading));
    }

    public static LocalDateTime? GetDateTime(this CsvRow csvRow, int index) {
        return GetDateTime(csvRow, CsvSelect.For(index));
    }

    public static LocalDateTime? GetDateTime(this CsvRow csvRow, CsvSelect select) {
        return csvRow.ParseField(select,
                                 (parser, field) => parser.DateTime
                                                          .Parse(field, OurDataTypes.DateTime.GetClrType()));
    }
}
