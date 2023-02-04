using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Email.Lookups;

public class BodyFormat : NamedLookup {
    public BodyFormat(string id, string name) : base(id, name) { }
}

public class BodyFormats : StaticLookupsCollection<BodyFormat> {
    public static readonly BodyFormat Html = new("html", "HTML");
    public static readonly BodyFormat Text = new("text", "Text");
}
