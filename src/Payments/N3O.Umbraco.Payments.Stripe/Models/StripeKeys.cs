namespace N3O.Umbraco.Payments.Stripe.Models {
    public class StripeKeys : Value {
        public StripeKeys(string client, string secret) {
            Client = client;
            Secret = secret;
        }

        public string Client { get; }
        public string Secret { get; }
    }
}