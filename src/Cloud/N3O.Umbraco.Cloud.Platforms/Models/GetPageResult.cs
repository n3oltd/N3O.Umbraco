using N3O.Umbraco.Extensions;
using Newtonsoft.Json;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class GetPageResult {
    private GetPageResult(PlatformsPage page, string redirectUrl) {
        Page = page;
        RedirectUrl = redirectUrl;
    }
    
    public PlatformsPage Page { get; }
    public string RedirectUrl { get; }
    
    [JsonIgnore]
    public bool IsRedirect => RedirectUrl.HasValue();

    public static GetPageResult ForPage(PlatformsPage page) {
        return new GetPageResult(page, null);
    }
    
    public static GetPageResult ForRedirect(string redirectUrl) {
        return new GetPageResult(null, redirectUrl);
    }
}