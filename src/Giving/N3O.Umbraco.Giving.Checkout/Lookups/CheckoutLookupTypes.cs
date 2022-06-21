using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Giving.Checkout.Lookups;

public class CheckoutLookupTypes : ILookupTypesSet {
    [LookupInfo(typeof(CheckoutStage))]
    public const string CheckoutStages = "checkoutStages";

    [LookupInfo(typeof(RegularGivingFrequency))]
    public const string RegularGivingFrequencies = "regularGivingFrequencies";
}
