using N3O.Umbraco.Attributes;

namespace N3O.Umbraco.Payments.PayPal.Models.PayPalCreatePlanSubscriptionReq;

public class PayPalCreateSubscriptionRes {
    [Name("Return Url")]
    public string ReturnUrl { get; set; }
    
    [Name("Cancel Url")]
    public string CancelUrl { get; set; }
}