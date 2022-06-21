namespace N3O.Umbraco.Payments.Stripe.Models;

public class StripeApiSettings : Value {
    public StripeApiSettings(string clientKey, string secretKey) {
        ClientKey = clientKey;
        SecretKey = secretKey;
    }

    public string ClientKey { get; }
    public string SecretKey { get; }
}
