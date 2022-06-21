namespace N3O.Umbraco.Payments.Opayo.Models;

public partial class OpayoPayment {
    public new void ThreeDSecureComplete(string res) {
        ClearErrors();
        
        base.ThreeDSecureComplete(res);
    }
}
