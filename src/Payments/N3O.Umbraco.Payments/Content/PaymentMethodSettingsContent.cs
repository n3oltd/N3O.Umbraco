using N3O.Umbraco.Content;

namespace N3O.Umbraco.Payments.Content {
    public class PaymentMethodSettingsContent<T> : UmbracoContent<T>, IPaymentMethodSettings
        where T : IPaymentMethodSettings {
        public string TransactionDescription => GetValue(x => x.TransactionDescription);
        public string TransactionId => GetValue(x => x.TransactionId);
    }
}