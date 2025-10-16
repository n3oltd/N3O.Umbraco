using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Cloud.Platforms.Lookups;

public class DonateButtonAction : NamedLookup {
    public DonateButtonAction(string id, string name) : base(id, name) { }
}

public class DonateButtonActions : StaticLookupsCollection<DonateButtonAction> {
    public static readonly DonateButtonAction AddToCart = new("addToCart", "Add To Cart");
    public static readonly DonateButtonAction BeginCheckout = new("beginCheckout", "Begin Checkout");
    public static readonly DonateButtonAction OpenDonationForm = new("openDonationForm", "Open Donation Form");
}