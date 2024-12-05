using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Elements.Lookups;

public class DonationCategoryType : Lookup {
    public DonationCategoryType(string id, int order) : base(id) {
        Order = order;
    }
    
    public int Order { get; }
}

public class DonationCategoryTypes : StaticLookupsCollection<DonationCategoryType> {
    public static readonly DonationCategoryType Dimension = new("dimension", 3);
    public static readonly DonationCategoryType Ephemeral = new("ephemeral", 1);
    public static readonly DonationCategoryType General = new("general", 2);
}
