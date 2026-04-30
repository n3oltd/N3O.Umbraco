using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Cloud.Platforms.Lookups;

public class ECommerceStage : NamedLookup {
    public ECommerceStage(string id, string name) : base(id, name) { }
}

public class ECommerceStages : StaticLookupsCollection<ECommerceStage> {
    public static readonly ECommerceStage Cart = new("cart", "Cart");
    public static readonly ECommerceStage Form = new("form", "Form");
}