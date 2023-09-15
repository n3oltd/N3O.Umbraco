using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Financial;
using N3O.Umbraco.Giving.Checkout.Lookups;

namespace N3O.Umbraco.Giving.Checkout.Extensions;

public static class CheckoutExtensions {
    public static Money GetStageValue(this Entities.Checkout checkout) {
        if (checkout.Progress.CurrentStage == CheckoutStages.Donation) {
            return checkout.Donation.Total;
        } else if (checkout.Progress.CurrentStage == CheckoutStages.RegularGiving) {
            return checkout.RegularGiving.Total;
        } else {
            throw UnrecognisedValueException.For(checkout.Progress.CurrentStage);
        }
    }
}