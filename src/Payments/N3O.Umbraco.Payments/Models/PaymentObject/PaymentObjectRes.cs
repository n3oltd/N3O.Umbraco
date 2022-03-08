using N3O.Umbraco.Payments.Lookups;

namespace N3O.Umbraco.Payments.Models {
    public class PaymentObjectRes {
        public PaymentObjectType Type { get; set; }
        public PaymentMethod Method { get; set; }
        public PaymentObjectStatus Status { get; set; }
        public string ErrorMessage { get; set; }

        public bool HasError { get; set; }
        public bool IsComplete { get; set; }
        public bool IsInProgress { get; set; }
    }
}