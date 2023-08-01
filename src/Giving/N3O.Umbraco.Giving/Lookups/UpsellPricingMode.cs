using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Giving.Lookups;

public class UpsellPricingMode : NamedLookup {
    public UpsellPricingMode(string id, string name) : base(id, name) { }
}

public class UpsellPricingModes : StaticLookupsCollection<UpsellPricingMode> {
    public static readonly UpsellPricingMode DonationItem = new("donationItem", "Donation Item");
    public static readonly UpsellPricingMode AnyAmount = new("anyAmount", "Any Amount");
    public static readonly UpsellPricingMode FixedAmount = new("fixedAmount", "Fixed Amount");
    public static readonly UpsellPricingMode PriceHandles = new("priceHandles", "Price Handles");
    public static readonly UpsellPricingMode Custom = new("custom", "Custom");
}