using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Lookups;
using System;

namespace N3O.Umbraco.Payments.Lookups;

public abstract class PaymentMethod :  NamedLookup {
    private readonly Type _paymentObjectType;
    private readonly Type _credentialObjectType;

    protected PaymentMethod(string id, string name, Type paymentObjectType, Type credentialObjectType)
        : base(id, name) {
        _paymentObjectType = paymentObjectType;
        _credentialObjectType = credentialObjectType;
    }

    public Type GetObjectType(PaymentObjectType objectType) {
        if (objectType == PaymentObjectTypes.Credential) {
            return _credentialObjectType;
        } else if (objectType == PaymentObjectTypes.Payment) {
            return _paymentObjectType;
        } else {
            throw UnrecognisedValueException.For(objectType);
        }
    }
    
    public abstract string GetSettingsContentTypeAlias();

    public virtual bool IsAvailable(Country country, Currency currency) {
        return true;
    }

    public bool Supports(PaymentObjectType paymentObjectType) {
        return GetObjectType(paymentObjectType) != null;
    }
}

public class PaymentMethods : TypesLookupsCollection<PaymentMethod> { }
