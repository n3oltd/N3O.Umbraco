using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Accounts.Lookups;

public class AddressLayout : NamedLookup {
    public AddressLayout(string id, string name) : base(id, name) { }
}

public class AddressLayouts : StaticLookupsCollection<AddressLayout> {
    public static readonly AddressLayout Layout1 = new("layout1", "Layout 1");
    public static readonly AddressLayout Layout2 = new("layout2", "Layout 2");
}