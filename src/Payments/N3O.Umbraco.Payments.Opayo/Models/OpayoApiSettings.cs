namespace N3O.Umbraco.Payments.Opayo.Models {
    public class OpayoApiSettings : Value {
        public OpayoApiSettings(string baseUrl, string integrationKey, string integrationPassword, string vendorName) {
            BaseUrl = baseUrl;
            IntegrationKey = integrationKey;
            IntegrationPassword = integrationPassword;
            VendorName = vendorName;
        }

        public string BaseUrl { get; }
        public string IntegrationKey { get; }
        public string IntegrationPassword { get; }
        public string VendorName { get; }
    }
}