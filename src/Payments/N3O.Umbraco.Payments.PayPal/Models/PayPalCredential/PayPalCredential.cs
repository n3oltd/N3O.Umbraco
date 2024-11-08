using N3O.Umbraco.Payments.Lookups;
using N3O.Umbraco.Payments.Models;

namespace N3O.Umbraco.Payments.PayPal.Models;

public partial class PayPalCredential : Credential {
    public override PaymentMethod Method => PayPalConstants.PaymentMethod;
    
    public int? PayPalErrorCode { get; private set; }
    public string PayPalErrorMessage { get; private set; }
    public string PayPalSubscriptionId { get; private set; }
    public string PayPalSubscriptionReason { get; private set; }
}