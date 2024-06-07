using FluentValidation;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Validation;
using System.Threading.Tasks;

namespace N3O.Umbraco.Captcha.Models;

public class CaptchaReqValidator : ModelValidator<CaptchaReq> {
    private readonly ICaptchaValidatorFactory _captchaValidatorFactory;

    public CaptchaReqValidator(IFormatter formatter, ICaptchaValidatorFactory captchaValidatorFactory) 
        : base(formatter) {
        _captchaValidatorFactory = captchaValidatorFactory;
        
        RuleFor(x => x.Token)
            .NotEmpty()
            .WithMessage(Get<Strings>(s => s.SpecifyToken));

        RuleFor(x => x.Token)
           .MustAsync(async (req, _, _) => await ValidateTokenAsync(req))
           .WithMessage(Get<Strings>(s => s.Invalid));
    }
    
    private async Task<bool> ValidateTokenAsync(CaptchaReq req) {
        var captchaValidator = _captchaValidatorFactory.CreateValidator();
        
        var isValid = await captchaValidator.IsValidAsync(req.Token, req.Action);

        return isValid;
    }
    
    public class Strings : ValidationStrings {
        public string Invalid => "Invalid CAPTCHA. Please try again.";
        public string SpecifyToken => "CAPTCHA token must be specified";
    }
}
