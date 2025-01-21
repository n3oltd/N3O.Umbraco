using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Accounts.Lookups;

public class AddressLayout : NamedLookup {
    public AddressLayout(string id, string name, string icon) : base(id, name) {
        Icon = icon;
    }
    
    public string Icon { get; }
}

public class AddressLayouts : StaticLookupsCollection<AddressLayout> {
    public static readonly AddressLayout Layout1 = new("layout1", "Layout 1", "icon-mobile");
    public static readonly AddressLayout Layout2 = new("layout2", "Layout 2", "icon-mobile");
}