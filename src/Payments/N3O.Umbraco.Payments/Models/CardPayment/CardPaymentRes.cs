namespace N3O.Umbraco.Payments.Models {
    public class CardPaymentRes {
        public bool ThreeDSecureRequired { get; set; }
        public bool ThreeDSecureCompleted { get; set; }
        public string ThreeDSecureChallengeUrl { get; set; }
        public string ThreeDSecureAcsTransId { get; set; }
        public string ThreeDSecureCReq { get; set; }
        public string ThreeDSecureCRes { get; set; }
    }
}