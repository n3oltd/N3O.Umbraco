using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Data.Extensions;

public static partial class CsvRowExtensions {
    public static TLookup GetLookup<TLookup>(this CsvRow csvRow, string heading)
        where TLookup : class, INamedLookup {
        return GetLookup<TLookup>(csvRow, CsvSelect.For(heading));
    }
    
    public static TLookup GetLookup<TLookup>(this CsvRow csvRow, int index)
        where TLookup : class, INamedLookup {
        return GetLookup<TLookup>(csvRow, CsvSelect.For(index));
    }
    
    public static TLookup GetLookup<TLookup>(this CsvRow csvRow, CsvSelect select)
        where TLookup : class, INamedLookup {
        return csvRow.ParseField(select, (parser, field) => parser.Lookup.Parse<TLookup>(field));
    }
}
