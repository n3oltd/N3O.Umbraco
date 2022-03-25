using N3O.Umbraco.Payments.Lookups;
using N3O.Umbraco.Payments.Models;

namespace N3O.Umbraco.Payments.Bambora.Models {
    public partial class BamboraPayment : Payment {
        public int? BamboraErrorCode { get; set; }
        public string BamboraErrorMessage { get; set; }
        public string BamboraPaymentId { get; set; }
        public int? BamboraStatusCode { get; set; }
        public string BamboraStatusDetail { get; set; }
        public string BamboraToken { get; set; }
        public string ReturnUrl { get; set; }

        public override PaymentMethod Method => BamboraConstants.PaymentMethod;
    }
}