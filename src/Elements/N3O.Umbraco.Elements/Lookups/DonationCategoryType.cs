using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Elements.Lookups;

public class DonationCategoryType : NamedLookup {
    public DonationCategoryType(string id, string name) : base(id, name) { }
}

public class DonationCategoryTypes : StaticLookupsCollection<DonationCategoryType> {
    public static readonly DonationCategoryType DimensionCategory = new("dimensionCategory", "Dimension Category");
    public static readonly DonationCategoryType EphemeralCategory = new("ephemeralCategory", "Ephemeral Category");
    public static readonly DonationCategoryType DefaultCategory = new("defaultCategory", "Default Category");
}
