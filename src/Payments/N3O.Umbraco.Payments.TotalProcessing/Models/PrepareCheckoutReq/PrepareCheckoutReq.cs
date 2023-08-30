using N3O.Umbraco.Attributes;
using N3O.Umbraco.Financial;

namespace N3O.Umbraco.Payments.TotalProcessing.Models;

public class PrepareCheckoutReq {
    [Name("Value")]
    public MoneyReq Value { get; set; }
    
    [Name("Return Url")]
    public string ReturnUrl { get; set; }
}
