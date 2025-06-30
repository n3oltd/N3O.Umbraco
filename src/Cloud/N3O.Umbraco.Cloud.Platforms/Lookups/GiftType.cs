using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Cloud.Platforms.Lookups;

public class GiftType : NamedLookup {
    public GiftType(string id, string name, string icon) : base(id, name) {
        Icon = icon;
    }
    
    public string Icon { get; }
}

public class GiftTypes : StaticLookupsCollection<GiftType> {
    public static readonly GiftType OneTime = new("oneTime", "One Time", "icon-coin-dollar");
    public static readonly GiftType Recurring = new("recurring", "Recurring", "icon-coins-dollar");
}