using N3O.Umbraco.Payments.Lookups;
using N3O.Umbraco.Payments.Models;

namespace N3O.Umbraco.Payments.Worldline.Models;

public partial class WorldlineCredential : Credential {
    public override PaymentMethod Method => WorldlineConstants.PaymentMethod;
}
