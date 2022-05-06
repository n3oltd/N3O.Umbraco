using N3O.Umbraco.Content;
using N3O.Umbraco.Lookups;
using System.Collections.Generic;

namespace N3O.Umbraco.Payments.Content {
    public class PaymentMethodSettingsContent<T> : UmbracoContent<T>, IPaymentMethodSettings
        where T : IPaymentMethodSettings {
        public string TransactionDescription => GetValue(x => x.TransactionDescription);
        public string TransactionId => GetValue(x => x.TransactionId);
        public IEnumerable<DayOfMonth> RestrictCollectionDaysTo => GetValue(x => x.RestrictCollectionDaysTo);
    }
}