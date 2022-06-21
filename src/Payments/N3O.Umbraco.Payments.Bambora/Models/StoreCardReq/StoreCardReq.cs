using N3O.Umbraco.Attributes;
using N3O.Umbraco.Payments.Models;

namespace N3O.Umbraco.Payments.Bambora.Models;

public class StoreCardReq {
    [Name("Token")]
    public string Token { get; set; }

    [Name("Return URL")]
    public string ReturnUrl { get; set; }

    [Name("Browser Parameters")]
    public BrowserParametersReq BrowserParameters { get; set; }
}
