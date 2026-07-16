using N3O.Umbraco.Attributes;

namespace N3O.Umbraco.Payments.TotalProcessing.Models;

public class PrepareCheckoutReq {
    [Name("Return Url")]
    public string ReturnUrl { get; set; }
}
