namespace N3O.Umbraco.Payments.Opayo.Models {
    public class OpayoDataEntrySettings : Value {
        public OpayoDataEntrySettings(string baseUrl, string integrationKey) {
            BaseUrl = baseUrl;
            IntegrationKey = integrationKey;
        }

        public string BaseUrl { get; }
        public string IntegrationKey { get; }
    }
}