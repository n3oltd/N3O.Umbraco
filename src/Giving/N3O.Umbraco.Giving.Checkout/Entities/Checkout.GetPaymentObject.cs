using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Giving.Checkout.Lookups;
using N3O.Umbraco.Payments.Lookups;
using N3O.Umbraco.Payments.Models;

namespace N3O.Umbraco.Giving.Checkout.Entities {
    public partial class Checkout {
        public PaymentObject GetPaymentObject(PaymentObjectType type) {
            if (type == PaymentObjectTypes.Payment) {
                if (Progress.CurrentStage == CheckoutStages.Donation) {
                    return Donation.Payment;
                } else if (Progress.CurrentStage == CheckoutStages.RegularGiving) {
                    return RegularGiving.Credential.AdvancePayment;
                }
            } else if (type == PaymentObjectTypes.Credential) {
                return RegularGiving.Credential;
            }
            
            throw UnrecognisedValueException.For(type);
        }
    }
}