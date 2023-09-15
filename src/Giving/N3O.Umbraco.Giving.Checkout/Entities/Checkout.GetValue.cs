using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Checkout.Lookups;

namespace N3O.Umbraco.Giving.Checkout.Entities;

public partial class Checkout {
    public Money GetValue() {
        if (Progress.CurrentStage == CheckoutStages.Donation) {
            return Donation.Total;
        } else if (Progress.CurrentStage == CheckoutStages.RegularGiving) {
            return RegularGiving.Total;
        } else {
            throw UnrecognisedValueException.For(Progress.CurrentStage);
        }
    }
}
