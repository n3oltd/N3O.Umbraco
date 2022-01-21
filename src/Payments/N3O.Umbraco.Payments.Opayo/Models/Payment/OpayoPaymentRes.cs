namespace N3O.Umbraco.Payments.Opayo.Models {
    public class OpayoPaymentRes {
        public bool RequireThreeDSecure { get; set; }
        public ThreeDSecure ThreeDSecure { get; set; }
        public string TransactionId { get; set; }
        public bool IsDeclined { get; set; }
        public string DeclineReason { get; set; }
        public int? OpayoErrorCode { get; set; }
        public string OpayoErrorMessage { get; set; }
    }
}