using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Captcha;

public class CaptchaValidatorFactory : ICaptchaValidatorFactory {
    private readonly IEnumerable<ICaptchaValidator> _captchaValidators;

    public CaptchaValidatorFactory(IEnumerable<ICaptchaValidator> captchaValidators) {
        _captchaValidators = captchaValidators;
    }
    
    public ICaptchaValidator CreateValidator() {
        var validator = _captchaValidators.FirstOrDefault(x => x.CanValidate);

        return validator;
    }
}