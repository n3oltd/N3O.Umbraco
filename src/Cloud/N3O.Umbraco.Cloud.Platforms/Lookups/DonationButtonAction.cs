using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Cloud.Platforms.Lookups;

public class DonationButtonAction : NamedLookup {
    public DonationButtonAction(string id, string name) : base(id, name) { }
}

public class DonationButtonActions : StaticLookupsCollection<DonationButtonAction> {
    public static readonly DonationButtonAction AddToCart = new("addToCart", "Add To Cart");
    public static readonly DonationButtonAction BeginCheckout = new("beginCheckout", "Begin Checkout");
    public static readonly DonationButtonAction OpenDonationForm = new("openDonationForm", "Open Donation Form");
}