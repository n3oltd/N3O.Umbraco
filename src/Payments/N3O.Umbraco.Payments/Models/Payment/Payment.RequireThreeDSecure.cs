using N3O.Umbraco.Extensions;

namespace N3O.Umbraco.Payments.Models {
    public partial class Payment {
        protected void RequireThreeDSecure(string challengeUrl, string termUrl, string acsTransId, string cReq, string paReq) {
            var challenge = cReq.IfNotNull(x => new ChallengeThreeDSecure(acsTransId, x, null));
            var fallback = paReq.IfNotNull(x => new FallbackThreeDSecure(termUrl, x, null));
            
            Card = new CardPayment(true, false, challengeUrl, challenge, fallback);
        }
    }
}