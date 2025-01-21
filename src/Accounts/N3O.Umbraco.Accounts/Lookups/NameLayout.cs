using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Accounts.Lookups;

public class NameLayout : NamedLookup {
    public NameLayout(string id, string name) : base(id, name) { }
}

public class NameLayouts : StaticLookupsCollection<NameLayout> {
    public static readonly NameLayout Layout1 = new("layout1", "Layout 1");
    public static readonly NameLayout Layout2 = new("layout2", "Layout 2");
}