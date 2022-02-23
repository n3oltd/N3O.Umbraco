using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Lookups;
using N3O.Umbraco.Payments.Lookups;
using N3O.Umbraco.Payments.Models;
using System;

namespace N3O.Umbraco.Giving.Checkout.Lookups {
    public class CheckoutStage : NamedLookup {
        private readonly Func<Entities.Checkout, PaymentObject> _getPayment;
        private readonly Func<Entities.Checkout, PaymentObject> _getCredential;
        private readonly Func<Entities.Checkout, bool> _isComplete;

        public CheckoutStage(string id, string name, int order, Func<Entities.Checkout, PaymentObject> getPayment, Func<Entities.Checkout, PaymentObject> getCredential,  Func<Entities.Checkout, bool> isComplete)
            : base(id, name) {
            _getPayment = getPayment;
            _getCredential = getCredential;
            _isComplete = isComplete;
            Order = order;
        }
        
        public int Order { get; }

        public PaymentObject GetPaymentObject(Entities.Checkout checkout, PaymentObjectType type) {
            if (type == PaymentObjectTypes.Payment) {
                return _getPayment(checkout);
            } else if (type == PaymentObjectTypes.Credential) {
                return _getCredential(checkout);
            } else {
                throw UnrecognisedValueException.For(type);
            }
        }
        
        public bool IsComplete(Entities.Checkout checkout) => _isComplete(checkout);
    }

    public class CheckoutStages : StaticLookupsCollection<CheckoutStage> {
        public static readonly CheckoutStage Account = new("account",
                                                           "Account",
                                                           0,
                                                           c => null,
                                                           c => null,
                                                           c => c.Account.HasValue());

        public static readonly CheckoutStage Donation = new("donation",
                                                            "Donation",
                                                            2,
                                                            c => c.Donation.Payment,
                                                            c => null,
                                                            c => c.Donation.IsComplete);
        
        public static readonly CheckoutStage RegularGiving = new("regularGiving",
                                                                 "Regular Giving",
                                                                 1,
                                                                 c => c.RegularGiving.Credential?.AdvancePayment,
                                                                 c => c.RegularGiving.Credential,
                                                                 c => c.RegularGiving?.IsComplete ?? false);
    }
}