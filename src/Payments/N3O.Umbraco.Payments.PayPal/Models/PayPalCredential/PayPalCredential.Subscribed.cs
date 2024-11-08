using N3O.Umbraco.Payments.PayPal.Clients;

namespace N3O.Umbraco.Payments.PayPal.Models.PayPalCredential;

public partial class PayPalCredential {
    public void Subscribed(string subscriptionId, string reason) {
        ClearErrors();
        
        PayPalSubscriptionId = subscriptionId;
        PayPalSubscriptionReason = reason;
        
        SetUp();
    }
}