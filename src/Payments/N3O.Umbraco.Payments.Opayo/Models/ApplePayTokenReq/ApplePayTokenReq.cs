using N3O.Umbraco.Attributes;

namespace N3O.Umbraco.Payments.Opayo.Models;

public class ApplePayTokenReq {
    [Name("Application Data")]
    public string ApplicationData  { get; set; }
    
    [Name("Display Name")]
    public string DisplayName  { get; set; }
    
    [Name("Payment Data")]
    public string PaymentData  { get; set; }
    
    [Name("Session Validation Token")]
    public string SessionValidationToken { get; set; }
}