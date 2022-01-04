using N3O.Umbraco.Content;

namespace N3O.Umbraco.Captcha.Content {
    public class CaptchaSettings : UmbracoContent {
        public string SecretKey => GetValue<CaptchaSettings, string>(x => x.SecretKey);
        public double Threshold => GetValue<CaptchaSettings, double>(x => x.Threshold);
    }
}