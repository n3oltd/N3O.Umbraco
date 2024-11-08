namespace N3O.Umbraco.Payments.PayPal.Models;

public partial class PayPalCredential {
    public void SubscriptionCreated(string subscriptionId, string reason) {
        ClearErrors();
        
        PayPalSubscriptionId = subscriptionId;
        PayPalSubscriptionReason = reason;
        
        SetUp();
    }
}