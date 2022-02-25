using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Giving.Checkout.Lookups;
using N3O.Umbraco.Payments.Lookups;
using N3O.Umbraco.Payments.Models;

namespace N3O.Umbraco.Giving.Checkout.Entities {
    public partial class Checkout {
        public void SetPaymentObject(PaymentObjectType type, PaymentObject paymentObject) {
            if (type == PaymentObjectTypes.Payment) {
                if (Progress.CurrentStage == CheckoutStages.Donation) {
                    Donation = Donation.UpdatePayment((Payment) paymentObject);
                } else if (Progress.CurrentStage == CheckoutStages.RegularGiving) {
                    RegularGiving = RegularGiving.UpdateAdvancePayment((Payment) paymentObject);
                }
            } else if (type == PaymentObjectTypes.Credential) {
                RegularGiving = RegularGiving.UpdateCredential((Credential) paymentObject);
            }
            
            throw UnrecognisedValueException.For(type);
        }
    }
}