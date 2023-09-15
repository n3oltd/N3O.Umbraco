using N3O.Umbraco.Payments.Lookups;
using N3O.Umbraco.Payments.Models;

namespace N3O.Umbraco.Payments.TotalProcessing.Models;

public partial class TotalProcessingCredential : Credential {
    public string ReturnUrl { get; private set; }
    
    public string TotalProcessingCheckoutId { get; private set; }
    
    public string TotalProcessingUniqueReference { get; private set; }
    
    public string TotalProcessingRegistrationId { get; private set; }
    public string TotalProcessingErrorCode { get; private set; }
    public string TotalProcessingErrorMessage { get; private set; }
    
    public override PaymentMethod Method => TotalProcessingConstants.PaymentMethod;
}
