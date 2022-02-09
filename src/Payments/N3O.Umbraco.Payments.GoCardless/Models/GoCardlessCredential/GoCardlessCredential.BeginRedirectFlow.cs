namespace N3O.Umbraco.Payments.GoCardless.Models {
    public partial class GoCardlessCredential {
        public void BeginRedirectFlow(string redirectFlowId, string sessionToken, string returnUrl) {
            RedirectFlowId = redirectFlowId;
            SessionToken = sessionToken;
            ReturnUrl = returnUrl;
        }
    }
}