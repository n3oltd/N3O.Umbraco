using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Cloud.Platforms.Lookups;

public class DonateButtonAction : NamedLookup {
    public DonateButtonAction(string id, string name, string icon) : base(id, name) {
        Icon = icon;
    }
    
    public string Icon { get; }
}

public class DonateButtonActions : StaticLookupsCollection<DonateButtonAction> {
    public static readonly DonateButtonAction AddToCart = new("addToCart", "Add To Cart", "icon-shopping-basket");
    public static readonly DonateButtonAction BeginCheckout = new("beginCheckout", "Begin Checkout", "icon-donate");
    public static readonly DonateButtonAction OpenDonationForm = new("openDonationForm", "Open Donation Form", "icon-document");
}