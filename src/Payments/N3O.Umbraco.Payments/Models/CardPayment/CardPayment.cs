using Newtonsoft.Json;
using NUglify.Helpers;
using StackExchange.Profiling.Internal;
using System;

namespace N3O.Umbraco.Payments.Models {
    public class CardPayment : Value {
        [JsonConstructor]
        public CardPayment(bool threeDSecureRequired,
                           bool threeDSecureComplete,
                           string threeDSecureUrl,
                           ChallengeThreeDSecure challenge,
                           FallbackThreeDSecure fallback) {
            ThreeDSecureRequired = threeDSecureRequired;
            ThreeDSecureCompleted = threeDSecureComplete;
            ThreeDSecureUrl = threeDSecureUrl;

            Challenge = challenge;
            Fallback = fallback;
        }

        public CardPayment() : this(false, false, null, null, null) { }

        public bool ThreeDSecureRequired { get; }
        public bool ThreeDSecureCompleted { get; }
        public string ThreeDSecureUrl { get; }
        public ChallengeThreeDSecure Challenge { get; }
        public FallbackThreeDSecure Fallback { get; }

        public CardPayment ThreeDSecureComplete(string res) {
            if (!ThreeDSecureRequired || ThreeDSecureCompleted) {
                throw new InvalidOperationException();
            }

            var fallback = Fallback.IfNotNull(x => new FallbackThreeDSecure(x.TermUrl, x.PaReq, res));
            var challenge = Challenge.IfNotNull(x => new ChallengeThreeDSecure(x.AcsTransId, x.CReq, res));
            
            return new CardPayment(true,
                                   true,
                                   ThreeDSecureUrl,
                                   challenge,
                                   fallback);
        }
    }
}