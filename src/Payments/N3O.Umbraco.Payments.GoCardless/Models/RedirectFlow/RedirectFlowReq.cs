using N3O.Umbraco.Attributes;

namespace N3O.Umbraco.Payments.GoCardless.Models;

public class RedirectFlowReq {
    [Name("Return URL")]
    public string ReturnUrl { get; set; }
}
