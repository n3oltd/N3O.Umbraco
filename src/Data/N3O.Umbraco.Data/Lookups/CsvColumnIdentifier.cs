using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Data.Lookups {
    public class CsvColumnIdentifier : NamedLookup {
        public CsvColumnIdentifier(string id, string name) : base(id, name) { }
    }

    [StaticLookups]
    public class CsvColumnIdentifiers : StaticLookupsCollection<CsvColumnIdentifier> {
        public static readonly CsvColumnIdentifier Heading = new("heading", "Heading");
        public static readonly CsvColumnIdentifier Index = new("index", "Index");
    }
}