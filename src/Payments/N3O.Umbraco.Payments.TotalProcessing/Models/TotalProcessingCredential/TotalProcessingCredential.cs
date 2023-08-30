using N3O.Umbraco.Payments.Lookups;
using N3O.Umbraco.Payments.Models;

namespace N3O.Umbraco.Payments.TotalProcessing.Models;

public partial class TotalProcessingCredential : Credential {
    public override PaymentMethod Method => TotalProcessingConstants.PaymentMethod;
}
