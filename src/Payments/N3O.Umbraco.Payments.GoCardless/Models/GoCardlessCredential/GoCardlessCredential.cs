using N3O.Umbraco.Payments.Lookups;
using N3O.Umbraco.Payments.Models;

namespace N3O.Umbraco.Payments.GoCardless.Models {
    public partial class GoCardlessCredential : Credential {
        public override PaymentMethod Method => GoCardlessConstants.PaymentMethod;

        public string GoCardlessCustomerId { get; private set; }
        public string GoCardlessRedirectFlowId { get; private set; }
        public string GoCardlessMandateId { get; private set; }
        public string GoCardlessSessionToken { get; private set; }
        public string GoCardlessRedirectUrl { get; private set; }
        public string ReturnUrl { get; private set; }
    }
}