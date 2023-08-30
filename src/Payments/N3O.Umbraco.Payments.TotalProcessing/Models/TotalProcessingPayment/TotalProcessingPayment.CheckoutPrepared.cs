namespace N3O.Umbraco.Payments.TotalProcessing.Models;

public partial class TotalProcessingPayment {
    public void CheckoutPrepared(string returnUrl, string checkoutId, string uniqueReference) {
        ClearErrors();
        
        TotalProcessingCheckoutId = checkoutId;
        TotalProcessingUniqueReference = uniqueReference;

        ReturnUrl = returnUrl;
    }
}
