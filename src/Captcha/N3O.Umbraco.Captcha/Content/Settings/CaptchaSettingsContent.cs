using N3O.Umbraco.Content;

namespace N3O.Umbraco.Captcha.Content {
    public class CaptchaSettingsContent : UmbracoContent<CaptchaSettingsContent> {
        public string SecretKey => GetValue(x => x.SecretKey);
        public double Threshold => GetValue(x => x.Threshold);
    }
}