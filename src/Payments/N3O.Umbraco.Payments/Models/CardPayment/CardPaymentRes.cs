namespace N3O.Umbraco.Payments.Models {
    public class CardPaymentRes {
        public bool ThreeDSecureRequired { get; set; }
        public bool ThreeDSecureCompleted { get; set; }
        public ThreeDSecureV1 ThreeDSecureV1 { get; set; }
        public ThreeDSecureV2 ThreeDSecureV2 { get; set; }
    }
}