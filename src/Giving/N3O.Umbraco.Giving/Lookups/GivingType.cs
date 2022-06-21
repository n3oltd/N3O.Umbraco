using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Giving.Lookups;

public class GivingType : NamedLookup {
    public GivingType(string id, string name, string icon, int order, int valueMultiplier) : base(id, name) {
        Icon = icon;
        Order = order;
        ValueMultiplier = valueMultiplier;
    }

    public string Icon { get; }
    public int Order { get; }
    public int ValueMultiplier { get; }
}

public class GivingTypes : StaticLookupsCollection<GivingType> {
    public static readonly GivingType Donation = new("donation", "Donation", "icon-coin-dollar", 0, 1);
    public static readonly GivingType RegularGiving = new("regularGiving", "Regular Giving", "icon-coins-dollar", 1, 12);
}
