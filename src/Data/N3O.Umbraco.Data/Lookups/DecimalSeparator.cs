using N3O.Umbraco.Localization;
using N3O.Umbraco.Lookups;
using System.Text.RegularExpressions;

namespace N3O.Umbraco.Data.Lookups;

public class DecimalSeparator : NamedLookup {
    public DecimalSeparator(string id, string name, string separator, NumberFormat exampleFormat)
        : base(id, name) {
        Separator = separator;
        ExampleFormat = exampleFormat;
    }

    public string Separator { get; }
    public string SeparatorRegex => Regex.Escape(Separator);
    public NumberFormat ExampleFormat { get; }
}

public class DecimalSeparators : StaticLookupsCollection<DecimalSeparator> {
    public static readonly DecimalSeparator Comma = new("comma",
                                                        "Comma",
                                                        DataConstants.DecimalSeparators.Comma,
                                                        NumberFormats.EU1);

    public static readonly DecimalSeparator Point = new("point",
                                                        "Point",
                                                        DataConstants.DecimalSeparators.Point,
                                                        NumberFormats.International);
}
