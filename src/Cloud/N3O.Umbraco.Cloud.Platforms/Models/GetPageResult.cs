using N3O.Umbraco.Extensions;
using N3O.Umbraco.Redirects;
using Newtonsoft.Json;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class GetPageResult {
    private GetPageResult(PlatformsPage page, Redirect redirect) {
        Page = page;
        Redirect = redirect;
    }
    
    public PlatformsPage Page { get; }
    public Redirect Redirect { get; }
    
    [JsonIgnore]
    public bool IsRedirect => Redirect.HasValue();

    public static GetPageResult ForPage(PlatformsPage page) {
        return new GetPageResult(page, null);
    }
    
    public static GetPageResult ForRedirect(string url, bool temporary) {
        return new GetPageResult(null, new Redirect(url, temporary));
    }
}