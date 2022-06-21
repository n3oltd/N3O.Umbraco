using N3O.Umbraco.Webhooks.Lookups;

namespace N3O.Umbraco.Giving.Checkout.Lookups;

public class CheckoutWebhookEvents : IWebhookEvents {
    public static readonly WebhookEvent DonationCompleteEvent = new("donationComplete",
                                                                    "Donation Complete",
                                                                    "Raised when a donation is complete",
                                                                    "icon-cash-register");
    
    public static readonly WebhookEvent RegularGivingCompleteEvent = new("regularGivingComplete",
                                                                         "Regular Giving Complete",
                                                                         "Raised when a regular giving is complete",
                                                                         "icon-cash-register");
}
