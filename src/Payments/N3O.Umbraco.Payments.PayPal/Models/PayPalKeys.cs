namespace N3O.Umbraco.Payments.PayPal.Models {
    public class PayPalKeys {
        public PayPalKeys(string client) {
            Client = client;
        }

        public string Client { get; }
    }
}