namespace N3O.Umbraco.Payments.Bambora.Models;

public partial class BamboraPayment {
    public new void ThreeDSecureComplete(string cRes) {
        ClearErrors();
        
        base.ThreeDSecureComplete(cRes);
    }
}
