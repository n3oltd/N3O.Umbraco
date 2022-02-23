using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Lookups;
using N3O.Umbraco.Payments.Lookups;
using N3O.Umbraco.Payments.Models;
using System;

namespace N3O.Umbraco.Giving.Checkout.Lookups {
    public class CheckoutStage : NamedLookup {
        private readonly Func<Entities.Checkout, Payment> _getPayment;
        private readonly Func<Entities.Checkout, Credential> _getCredential;
        private readonly Func<Entities.Checkout, bool> _isComplete;

        public CheckoutStage(string id,
                             string name,
                             Func<Entities.Checkout, Payment> getPayment,
                             Func<Entities.Checkout, Credential> getCredential,
                             Func<Entities.Checkout, bool> isComplete,
                             int order)
            : base(id, name) {
            _getPayment = getPayment;
            _getCredential = getCredential;
            _isComplete = isComplete;
            Order = order;
        }
        
        public int Order { get; }

        public PaymentObject GetPaymentObject(Entities.Checkout checkout, PaymentObjectType type) {
            try {
                PaymentObject paymentObject;
                
                if (type == PaymentObjectTypes.Payment) {
                    paymentObject = _getPayment(checkout);
                } else if (type == PaymentObjectTypes.Credential) {
                    paymentObject = _getCredential(checkout);
                } else {
                    throw UnrecognisedValueException.For(type);
                }

                return paymentObject;
            } catch {
                throw new Exception($"Payment object of type {type} is not available at the {Name} stage");
            }
        }
        
        public bool IsComplete(Entities.Checkout checkout) => _isComplete(checkout);
    }

    public class CheckoutStages : StaticLookupsCollection<CheckoutStage> {
        public static readonly CheckoutStage Account = new("account",
                                                           "Account",
                                                           _ => throw new InvalidOperationException(),
                                                           _ => throw new InvalidOperationException(),
                                                           c => c.Account.HasValue(),
                                                           0);

        public static readonly CheckoutStage Donation = new("donation",
                                                            "Donation",
                                                            c => c.Donation.Payment,
                                                            _ => throw new InvalidOperationException(),
                                                            c => c.Donation.IsComplete,
                                                            2);
        
        public static readonly CheckoutStage RegularGiving = new("regularGiving",
                                                                 "Regular Giving",
                                                                 c => c.RegularGiving.Credential?.AdvancePayment,
                                                                 c => c.RegularGiving.Credential,
                                                                 c => c.RegularGiving.IsComplete,
                                                                 1);
    }
}