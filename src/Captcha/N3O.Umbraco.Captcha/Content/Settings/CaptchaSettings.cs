using N3O.Umbraco.Content;

namespace N3O.Umbraco.Captcha.Content {
    public class CaptchaSettings : UmbracoContent<CaptchaSettings> {
        public string SecretKey => GetValue(x => x.SecretKey);
        public double Threshold => GetValue(x => x.Threshold);
    }
}