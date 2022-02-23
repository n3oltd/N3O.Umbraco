using Newtonsoft.Json;
using System;

namespace N3O.Umbraco.Payments.Models {
    public class CardPayment : Value {
        [JsonConstructor]
        public CardPayment(bool threeDSecureRequired,
                           bool threeDSecureComplete,
                           string threeDSecureChallengeUrl,
                           string threeDSecureAcsTransId,
                           string threeDSecureCReq,
                           string threeDSecureCRes) {
            ThreeDSecureRequired = threeDSecureRequired;
            ThreeDSecureCompleted = threeDSecureComplete;
            ThreeDSecureChallengeUrl = threeDSecureChallengeUrl;
            ThreeDSecureAcsTransId = threeDSecureAcsTransId;
            ThreeDSecureCReq = threeDSecureCReq;
            ThreeDSecureCRes = threeDSecureCRes;
        }

        public CardPayment() : this(false, false, null, null, null, null) { }

        public bool ThreeDSecureRequired { get; }
        public bool ThreeDSecureCompleted { get; }
        public string ThreeDSecureChallengeUrl { get; }
        public string ThreeDSecureAcsTransId { get; }
        public string ThreeDSecureCReq { get; }
        public string ThreeDSecureCRes { get; }

        public CardPayment ThreeDSecureComplete(string cRes) {
            if (!ThreeDSecureRequired || ThreeDSecureCompleted) {
                throw new InvalidOperationException();
            }

            return new CardPayment(true,
                                   true,
                                   ThreeDSecureChallengeUrl,
                                   ThreeDSecureAcsTransId,
                                   ThreeDSecureCReq,
                                   cRes);
        }
    }
}