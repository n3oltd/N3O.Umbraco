using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Giving.Checkout.Lookups;
using N3O.Umbraco.Payments.Lookups;
using N3O.Umbraco.Payments.Models;
using System;

namespace N3O.Umbraco.Giving.Checkout.Entities {
    public partial class Checkout {
        public PaymentObject GetPaymentObject(PaymentObjectType type) {
            if (type == PaymentObjectTypes.Payment) {
                return GetPaymentObject(type, CheckoutStages.Donation, () => Donation.Payment);
            } else if (type == PaymentObjectTypes.Credential) {
                return GetPaymentObject(type, CheckoutStages.RegularGiving, () => RegularGiving.Credential);
            } else {
                throw UnrecognisedValueException.For(type);
            }
        }

        private PaymentObject GetPaymentObject(PaymentObjectType paymentObjectType,
                                               CheckoutStage stage,
                                               Func<PaymentObject> get) {
            if (Progress.CurrentStage != stage) {
                throw new Exception($"Payment object of type {paymentObjectType} is not available at the {Progress.CurrentStage} stage");
            } else {
                return get();
            }
        }
    }
}