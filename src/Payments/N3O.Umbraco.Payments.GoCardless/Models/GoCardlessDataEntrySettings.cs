namespace N3O.Umbraco.Payments.GoCardless.Models {
    public class GoCardlessDataEntrySettings {
        public GoCardlessDataEntrySettings(string baseUrl, string redirectFlowUrl) {
            RedirectFlowUrl = redirectFlowUrl;
            BaseUrl = baseUrl;
        }

        public string BaseUrl { get; }
        public string RedirectFlowUrl { get; }
    }
}