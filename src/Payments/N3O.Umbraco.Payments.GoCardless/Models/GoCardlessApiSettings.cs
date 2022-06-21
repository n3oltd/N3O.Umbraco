using GoCardless;

namespace N3O.Umbraco.Payments.GoCardless.Models;

public class GoCardlessApiSettings : Value {
    public GoCardlessApiSettings(string accessToken, GoCardlessClient.Environment environment) {
        AccessToken = accessToken;
        Environment = environment;
    }

    public string AccessToken { get; }
    public GoCardlessClient.Environment Environment { get; }
}
