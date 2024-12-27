using N3O.Umbraco.Content;

namespace N3O.Umbraco.Cdn.Cloudflare.Content;

public class CloudflareSettingsContent : UmbracoContent<CloudflareSettingsContent> {
    public string VideoAccountId => GetValue(x => x.VideoAccountId);
    public string VideoToken => GetValue(x => x.VideoToken);
}
