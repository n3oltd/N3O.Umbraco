namespace N3O.Umbraco.Captcha;

public interface ICaptchaValidatorFactory {
    ICaptchaValidator CreateValidator();
}