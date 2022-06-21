using N3O.Umbraco.Attributes;

namespace N3O.Umbraco.Payments.PayPal.Models;

public class PayPalTransactionReq {
    [Name("Email")]
    public string Email { get; set; }
    
    [Name("Authorization ID")]
    public string AuthorizationId { get; set; }
}
