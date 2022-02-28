using N3O.Umbraco.Payments.Lookups;

namespace N3O.Umbraco.Payments {
    public interface IPaymentMethodDataEntryConfiguration<in T> where T : PaymentMethod {
        object GetConfiguration(T paymentMethod);
    }
}