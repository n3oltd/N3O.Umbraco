using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Giving.Checkout.Lookups;
using N3O.Umbraco.Payments.Lookups;
using N3O.Umbraco.Payments.Models;
using System;

namespace N3O.Umbraco.Giving.Checkout.Entities {
    public partial class Checkout {
        public void SetPaymentObject(PaymentObjectType type, PaymentObject paymentObject) {
            if (type == PaymentObjectTypes.Payment) {
                SetPaymentObject<Payment>(type,
                                          paymentObject,
                                          CheckoutStages.Donation,
                                          payment => Donation = Donation.UpdatePayment(payment));
            } else if (type == PaymentObjectTypes.Credential) {
                SetPaymentObject<Credential>(type,
                                             paymentObject,
                                             CheckoutStages.RegularGiving,
                                             credential => RegularGiving = RegularGiving.UpdateCredential(credential));
            } else {
                throw UnrecognisedValueException.For(type);
            }
        }

        private void SetPaymentObject<T>(PaymentObjectType paymentObjectType,
                                         PaymentObject paymentObject,
                                         CheckoutStage stage,
                                         Action<T> set)
            where T : class {
            if (Progress.CurrentStage != stage) {
                throw new Exception($"Payment object of type {paymentObjectType} is not available at the {Progress.CurrentStage} stage");
            } else {
                set(paymentObject as T);
            }
        }
    }
}