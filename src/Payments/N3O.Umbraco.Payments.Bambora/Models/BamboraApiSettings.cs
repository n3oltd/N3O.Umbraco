namespace N3O.Umbraco.Payments.Bambora.Models {
    public class BamboraApiSettings : Value {
        public BamboraApiSettings(string merchantId, string passcode) {
            MerchantId = merchantId;
            Passcode = passcode;
        }

        public string MerchantId { get; }
        public string Passcode { get; }
    }
}