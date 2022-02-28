namespace N3O.Umbraco.Payments.Stripe.Models {
    public class StripeDataEntrySettings {
        public StripeDataEntrySettings(string clientKey) {
            ClientKey = clientKey;
        }

        public string ClientKey { get; }
    }
}