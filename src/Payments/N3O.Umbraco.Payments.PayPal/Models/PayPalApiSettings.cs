namespace N3O.Umbraco.Payments.PayPal.Models;

public class PayPalApiSettings {
    public PayPalApiSettings(string baseUrl, string accessToken, string clientId) {
        BaseUrl = baseUrl;
        ClientId = clientId;
        AccessToken = accessToken;
    }

    public string BaseUrl { get; }
    public string AccessToken { get; }
    public string ClientId { get; }
}
