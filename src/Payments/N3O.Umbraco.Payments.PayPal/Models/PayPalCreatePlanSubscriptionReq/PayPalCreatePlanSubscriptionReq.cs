using N3O.Umbraco.Attributes;

namespace N3O.Umbraco.Payments.PayPal.Models.PayPalCreatePlanSubscriptionReq;

public class PayPalCreatePlanSubscriptionReq {
    [Name("Plan Name")]
    public string PlanName { get; set; }
}