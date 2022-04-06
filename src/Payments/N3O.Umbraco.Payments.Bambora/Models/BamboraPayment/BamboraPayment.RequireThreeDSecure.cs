using N3O.Umbraco.Payments.Models;

namespace N3O.Umbraco.Payments.Bambora.Models {
    public partial class BamboraPayment {
        public void RequireThreeDSecure(string returnUrl, string html, string sessionData) {
            ClearErrors();
            
            ReturnUrl = returnUrl;
            
            RequireThreeDSecureV2(ThreeDSecureV2.FromHtml(html, sessionData));
        }
    }
}