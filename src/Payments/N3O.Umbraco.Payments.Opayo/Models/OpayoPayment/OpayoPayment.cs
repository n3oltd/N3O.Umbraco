using N3O.Umbraco.Payments.Lookups;
using N3O.Umbraco.Payments.Models;

namespace N3O.Umbraco.Payments.Opayo.Models {
    public partial class OpayoPayment : Payment {
        public string TransactionId { get; private set; }
        public int? OpayoErrorCode { get; private set; }
        public string OpayoErrorMessage { get; private set; }
        public string OpayoStatusDetail { get; private set; }
        public long? OpayoRetrievalReference { get; private set; }
        public string BankAuthorisationCode { get; private set; }
        public string ThreeDSecureUrl { get; private set; }
        public string AcsTransId { get; private set; }
        public string CReq { get; private set; }
        public string CallbackUrl { get; set; }

        // TAdd it on propeties not needed on front end. [ExcludeFromSchema]
        public bool ThreeDSecureCompleted { get; private set; }

        public override PaymentMethod Method => OpayoConstants.PaymentMethod;
    }
}