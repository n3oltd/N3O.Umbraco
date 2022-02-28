using N3O.Umbraco.Payments.Lookups;

namespace N3O.Umbraco.Payments {
    public abstract class PaymentMethodDataEntryConfiguration<TMethod, TConfig> : IPaymentMethodDataEntryConfiguration<TMethod> where TMethod : PaymentMethod {
        public object GetConfiguration(TMethod paymentMethod) {
            return GetConfig(paymentMethod);
        }

        protected abstract TConfig GetConfig(TMethod paymentMethod);
    }
}