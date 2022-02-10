using N3O.Umbraco.Content;
using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Lookups;
using System;

namespace N3O.Umbraco.Payments.Lookups {
    public abstract class PaymentMethod :  NamedLookup {
        private readonly Type _paymentObjectType;
        private readonly Type _credentialObjectType;

        protected PaymentMethod(string id,
                                string name,
                                bool isCardPayment,
                                Type paymentObjectType,
                                Type credentialObjectType)
            : base(id, name) {
            IsCardPayment = isCardPayment;
            
            _paymentObjectType = paymentObjectType;
            _credentialObjectType = credentialObjectType;
        }
        
        public bool IsCardPayment { get; }

        public Type GetObjectType(PaymentObjectType objectType) {
            if (objectType == PaymentObjectTypes.Credential) {
                return _credentialObjectType;
            } else if (objectType == PaymentObjectTypes.Payment) {
                return _paymentObjectType;
            } else {
                throw UnrecognisedValueException.For(objectType);
            }
        }

        public virtual bool IsAvailable(IContentCache contentCache, Country country, Currency currency) {
            return true;
        }
    }

    public class PaymentMethods : TypesLookupsCollection<PaymentMethod> { }
}