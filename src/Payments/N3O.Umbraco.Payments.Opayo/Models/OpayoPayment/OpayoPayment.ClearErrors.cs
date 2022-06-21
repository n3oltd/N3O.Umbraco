namespace N3O.Umbraco.Payments.Opayo.Models;

public partial class OpayoPayment {
    protected override void ClearErrors() {
        base.ClearErrors();
        
        OpayoErrorCode = null;
        OpayoErrorMessage = null;
    }
}
