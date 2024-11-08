using N3O.Umbraco.Attributes;

namespace N3O.Umbraco.Payments.PayPal.Models.PayPalCreatePlanSubscriptionReq;

public class PayPalCreateSubscriptionRes {
    [Name("Subscription ID")]
    public string SubscriptionID { get; set; }
}