using N3O.Umbraco.Data.Lookups;

namespace N3O.Umbraco.Data.Models;

public class CsvSelect {
    public CsvSelect(CsvColumnIdentifier columnIdentifier, string heading, int? index) {
        ColumnIdentifier = columnIdentifier;
        Heading = heading;
        Index = index;
    }

    public CsvColumnIdentifier ColumnIdentifier { get; }
    public string Heading { get; }
    public int? Index { get; }

    public static CsvSelect For(string heading) => new(CsvColumnIdentifiers.Heading, heading, null);
    public static CsvSelect For(int index) => new(CsvColumnIdentifiers.Index, null, index);
}
