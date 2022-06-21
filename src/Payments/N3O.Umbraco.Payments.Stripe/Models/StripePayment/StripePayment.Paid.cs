namespace N3O.Umbraco.Payments.Stripe.Models;

public partial class StripePayment {
    private void Paid(string chargeId) {
        ClearErrors();
        
        StripeChargeId = chargeId;
        
        Paid();
    }
}
