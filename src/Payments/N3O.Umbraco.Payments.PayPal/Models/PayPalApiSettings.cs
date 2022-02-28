namespace N3O.Umbraco.Payments.PayPal.Models {
    public class PayPalApiSettings {
        public PayPalApiSettings(string clientId) {
            ClientId = clientId;
        }

        public string ClientId { get; }
    }
}