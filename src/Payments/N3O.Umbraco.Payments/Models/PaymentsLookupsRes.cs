using N3O.Umbraco.Lookups;
using N3O.Umbraco.Payments.Lookups;
using System.Collections.Generic;

namespace N3O.Umbraco.Payments.Models {
    public class PaymentsLookupsRes : LookupsRes {
        [FromLookupType(PaymentsLookupTypes.PaymentMethods)]
        public IEnumerable<PaymentMethodRes> PaymentMethods { get; set; }
    }
}