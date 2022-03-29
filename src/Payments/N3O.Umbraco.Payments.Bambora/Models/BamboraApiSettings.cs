namespace N3O.Umbraco.Payments.Bambora.Models {
    public class BamboraApiSettings : Value {
        public BamboraApiSettings(string merchantId, string paymentPasscode, string profilePasscode) {
            MerchantId = merchantId;
            PaymentPasscode = paymentPasscode;
            ProfilePasscode = profilePasscode;
        }

        public string MerchantId { get; }
        public string PaymentPasscode { get; }
        public string ProfilePasscode { get; }
    }
}