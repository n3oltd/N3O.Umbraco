namespace N3O.Umbraco.Payments.TotalProcessing.Models;

public partial class TotalProcessingPayment {
    public void UpdateCheckoutId(string returnUrl, string ndc, string checkoutId){
        ClearErrors();
        
        TotalProcessingCheckoutId = checkoutId;
        TotalProcessingUniqueReference = ndc;

        ReturnUrl = returnUrl;
    }
}
