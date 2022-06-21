using FluentValidation;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;

namespace N3O.Umbraco.Captcha.Models;

public class CaptchaReqValidator : ModelValidator<CaptchaReq> {
    public CaptchaReqValidator(IFormatter formatter) : base(formatter) {
        RuleFor(x => x.Token)
            .NotEmpty()
            .WithMessage(Get<Strings>(s => s.SpecifyToken));
    }
    
    public class Strings : ValidationStrings {
        public string SpecifyToken => "CAPTCHA token must be specified";
    }
}
