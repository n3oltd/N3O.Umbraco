using N3O.Umbraco.Payments.Models;

namespace N3O.Umbraco.Payments.Bambora.Models {
    public partial class BamboraCredential {
        public void RequireThreeDSecure(string customerCode,
                                        string returnUrl,
                                        string challengeUrl,
                                        string threeDSessionData,
                                        string threeDContent) {
            ClearErrors();
            
            BamboraCustomerCode = customerCode;

            ReturnUrl = returnUrl;
            
            CardPayment = new CardPayment(true, false, challengeUrl, threeDSessionData, threeDContent, null);

        }
    }
}