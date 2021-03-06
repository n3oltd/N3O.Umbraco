using N3O.Umbraco.Data.Models;
using N3O.Umbraco.References;
using OurDataTypes = N3O.Umbraco.Data.Lookups.DataTypes;

namespace N3O.Umbraco.Data.Extensions;

public static partial class CsvRowExtensions {
    public static Reference GetReference(this CsvRow csvRow, string heading) {
        return GetReference(csvRow, CsvSelect.For(heading));
    }

    public static Reference GetReference(this CsvRow csvRow, int index) {
        return GetReference(csvRow, CsvSelect.For(index));
    }

    public static Reference GetReference(this CsvRow csvRow, CsvSelect select) {
        return csvRow.ParseField(select,
                                 (parser, field) => parser.Reference.Parse(field,
                                                                           OurDataTypes.Reference.GetClrType()));
    }
}
