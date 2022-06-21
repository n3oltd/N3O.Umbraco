namespace N3O.Umbraco.Payments.Bambora.Models;

public partial class BamboraPayment {
    protected override void ClearErrors() {
        base.ClearErrors();
        
        BamboraErrorCode = null;
        BamboraErrorMessage = null;
    }
}
