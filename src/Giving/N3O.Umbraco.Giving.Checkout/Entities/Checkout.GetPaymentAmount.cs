using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Checkout.Extensions;
using N3O.Umbraco.Payments.Lookups;

namespace N3O.Umbraco.Giving.Checkout.Entities;

public partial class Checkout {
    public Money GetPaymentAmount(PaymentObjectType type) {
        if (type == PaymentObjectTypes.Payment) {
            return this.GetStageValue();
        } else if (type == PaymentObjectTypes.Credential) {
            return RegularGiving.Total;
        }

        throw UnrecognisedValueException.For(type);
    }
}
