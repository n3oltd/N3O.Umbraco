using N3O.Umbraco.Payments.Lookups;
using N3O.Umbraco.Payments.Models;

namespace N3O.Umbraco.Payments.Opayo.Models;

public partial class OpayoCredential : Credential {
    public override PaymentMethod Method => OpayoConstants.PaymentMethod;
}
