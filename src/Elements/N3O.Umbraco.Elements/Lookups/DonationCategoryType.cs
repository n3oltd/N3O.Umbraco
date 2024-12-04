using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Elements.Lookups;

public class DonationCategoryType : NamedLookup {
    public DonationCategoryType(string id, string name) : base(id, name) { }
}

public class DonationCategoryTypes : StaticLookupsCollection<DonationCategoryType> {
    public static readonly DonationCategoryType Dimension = new("dimension", "Dimension");
    public static readonly DonationCategoryType Ephemeral = new("ephemeral", "Ephemeral");
    public static readonly DonationCategoryType General = new("general", "General");
}
