using N3O.Umbraco.Payments.Lookups;
using N3O.Umbraco.Payments.Models;

namespace N3O.Umbraco.Payments.GoCardless.Models {
    public partial class GoCardlessCredential : Credential {
        public GoCardlessCredential() : base(null) { }
        
        public override PaymentMethod Method => GoCardlessConstants.PaymentMethod;

        public string SessionToken { get; private set; }
        public string RedirectFlowId { get; private set; }
        public string CustomerId { get; private set; }
        public string MandateId { get; private set; }
    }
}