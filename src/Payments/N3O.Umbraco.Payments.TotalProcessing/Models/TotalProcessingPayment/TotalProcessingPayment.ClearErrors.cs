namespace N3O.Umbraco.Payments.TotalProcessing.Models;

public partial class TotalProcessingPayment {
    protected override void ClearErrors() {
        base.ClearErrors();
        
        TotalProcessingErrorCode = null;
        TotalProcessingErrorMessage = null;
    }
}
