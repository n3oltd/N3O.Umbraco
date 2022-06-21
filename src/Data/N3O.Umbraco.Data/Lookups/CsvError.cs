using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Data.Lookups;

public class CsvError : Lookup {
    public CsvError(string id) : base(id) { }
}

[StaticLookups]
public class CsvErrors : StaticLookupsCollection<CsvError> {
    public static readonly CsvError MissingColumn = new("missingColumn");
    public static readonly CsvError ParsingFailed = new("parsingFailed");
}
