using N3O.Umbraco.Payments.Lookups;
using N3O.Umbraco.Payments.Models;

namespace N3O.Umbraco.Payments.Stripe.Models;

public partial class StripePayment : Payment {
    public string StripeChargeId { get; private set; }
    public string StripeCustomerId { get; private set; }
    public string StripeDeclineCode { get; private set; }
    public string StripeErrorCode { get; private set; }
    public string StripeErrorMessage { get; private set; }
    public string StripePaymentIntentId { get; private set; }
    public string StripePaymentIntentClientSecret { get; private set; }
    public string StripePaymentMethodId { get; private set; }

    public bool ActionRequired { get; private set; }
    
    public override PaymentMethod Method => StripeConstants.PaymentMethod;
}
