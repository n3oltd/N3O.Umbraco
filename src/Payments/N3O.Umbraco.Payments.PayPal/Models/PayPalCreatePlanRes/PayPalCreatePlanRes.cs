using N3O.Umbraco.Attributes;

namespace N3O.Umbraco.Payments.PayPal.Models.PayPalCreatePlanRes;

public class PayPalCreatePlanRes {
    [Name("Plan ID")]
    public string PlanId { get; set; }
}