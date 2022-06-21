using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Payments.Lookups;

public class PaymentsLookupTypes : ILookupTypesSet {
    [LookupInfo(typeof(PaymentMethod))]
    public const string PaymentMethods = "paymentMethods";
}
