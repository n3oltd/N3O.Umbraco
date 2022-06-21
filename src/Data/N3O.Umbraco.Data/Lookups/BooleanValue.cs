using N3O.Umbraco.Lookups;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Data.Lookups;

public class BooleanValue : NamedLookup {
    private readonly string[] _textValues;

    public BooleanValue(string id, string name, bool clrValue, params string[] textValues) : base(id, name) {
        ClrValue = clrValue;
        _textValues = textValues;
    }

    public bool ClrValue { get; }

    public override IEnumerable<string> GetTextValues() {
        var allTextValues = base.GetTextValues().Concat(_textValues).ToList();
        
        return allTextValues;
    }
}

[StaticLookups]
public class BooleanValues : StaticLookupsCollection<BooleanValue> {
    public static readonly BooleanValue True = new("true", "True", true, "yes", "y", "1");
    public static readonly BooleanValue False = new("false", "False", false, "no", "n", "0");
}
