using N3O.Umbraco.Attributes;

namespace N3O.Umbraco.Captcha.Models;

public class CaptchaReq {
    [Name("Token")]
    public string Token { get; set; }
    
    [Name("Action")]
    public string Action { get; set; }
}
