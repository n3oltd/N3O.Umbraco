namespace N3O.Umbraco.Payments.PayPal.Models {
    public class PayPalApiSettings {
        public PayPalApiSettings( string accessToken, string clientId, string baseUrl) {
            ClientId = clientId;
            BaseUrl = baseUrl;
            AccessToken = accessToken;
        }

        public string AccessToken { get; }
        public string ClientId { get; }
        public string BaseUrl { get; }
    }
}