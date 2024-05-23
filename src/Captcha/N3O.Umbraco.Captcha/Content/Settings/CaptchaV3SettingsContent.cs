using N3O.Umbraco.Content;

namespace N3O.Umbraco.Captcha.Content;

public class CaptchaV3SettingsContent : UmbracoContent<CaptchaV3SettingsContent> {
    public string SecretKey => GetValue(x => x.SecretKey);
    public string SiteKey => GetValue(x => x.SiteKey);
    public double Threshold => GetValue(x => x.Threshold);
}
