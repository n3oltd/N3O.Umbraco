using N3O.Umbraco.Content;

namespace N3O.Umbraco.Email.Amazon.Content;

public class AmazonSettingsContent : UmbracoContent<AmazonSettingsContent> {
    public string AccessKey => GetValue(x => x.AccessKey);
    public string SecretKey => GetValue(x => x.SecretKey);
    public string RegionCode => GetValue(x => x.RegionCode);
}
