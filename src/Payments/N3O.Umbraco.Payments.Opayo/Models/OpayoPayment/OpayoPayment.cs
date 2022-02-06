using N3O.Umbraco.Payments.Lookups;
using N3O.Umbraco.Payments.Models;

namespace N3O.Umbraco.Payments.Opayo.Models {
    public partial class OpayoPayment : Payment {
        public string OpayoTransactionId { get; private set; }
        public int? OpayoStatusCode { get; private set; }
        public string OpayoStatusDetail { get; private set; }
        public int? OpayoErrorCode { get; private set; }
        public string OpayoErrorMessage { get; private set; }
        public string OpayoBankAuthorisationCode { get; private set; }
        public long? OpayoRetrievalReference { get; private set; }
        public string ReturnUrl { get; private set; }

        public override PaymentMethod Method => OpayoConstants.PaymentMethod;
    }
}