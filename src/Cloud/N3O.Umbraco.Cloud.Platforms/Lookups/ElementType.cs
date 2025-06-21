using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Cloud.Platforms.Lookups;

public class ElementType : NamedLookup {
    public ElementType(string id, string name) : base(id, name) { }
}

public class ElementTypes : StaticLookupsCollection<ElementType> {
    public static readonly ElementType DonateButton = new("donateButton", "Donate Button");
    public static readonly ElementType DonationForm = new("donationForm", "Donation Form");
}