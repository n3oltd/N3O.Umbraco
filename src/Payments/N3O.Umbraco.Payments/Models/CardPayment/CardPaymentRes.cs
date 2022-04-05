namespace N3O.Umbraco.Payments.Models {
    public class CardPaymentRes {
        public bool ThreeDSecureRequired { get; set; }
        public bool ThreeDSecureCompleted { get; set; }
        public string ThreeDSecureUrl { get; set; }
        public ChallengeThreeDSecure Challenge { get; set; }
        public FallbackThreeDSecure Fallback { get; set; }
    }
}