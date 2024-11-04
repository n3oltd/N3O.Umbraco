using N3O.Umbraco.Payments.Lookups;
using N3O.Umbraco.Payments.Models;

namespace N3O.Umbraco.Payments.PayPal.Models.PayPalCredential;

public partial class PayPalCredential : Credential{
    public string PayPalEmail { get; private set; }
    public string PayPalSubscriptionId { get; private set; }
    
    public override PaymentMethod Method => PayPalConstants.PaymentMethod;
}