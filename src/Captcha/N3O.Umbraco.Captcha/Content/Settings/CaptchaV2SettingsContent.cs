using N3O.Umbraco.Content;

namespace N3O.Umbraco.Captcha.Content;

public class CaptchaV2SettingsContent : UmbracoContent<CaptchaV2SettingsContent> {
    public string SecretKey => GetValue(x => x.SecretKey);
    public string SiteKey => GetValue(x => x.SiteKey);
}
