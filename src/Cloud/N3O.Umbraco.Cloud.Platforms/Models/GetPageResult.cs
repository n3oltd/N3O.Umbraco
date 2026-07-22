using N3O.Umbraco.Extensions;
using N3O.Umbraco.Redirects;
using Newtonsoft.Json;

namespace N3O.Umbraco.Cloud.Platforms.Models;

public class GetPageResult {
    private GetPageResult(PlatformsPage page, Redirect redirect, bool isError) {
        Page = page;
        Redirect = redirect;
        IsError = isError;
    }

    public PlatformsPage Page { get; }
    public Redirect Redirect { get; }
    // True when the page could not be resolved due to a transient CDN infrastructure failure (not a 404)
    public bool IsError { get; }

    [JsonIgnore]
    public bool IsRedirect => Redirect.HasValue();

    public static GetPageResult ForPage(PlatformsPage page) {
        return new GetPageResult(page, null, false);
    }

    public static GetPageResult ForRedirect(string url, bool temporary) {
        return new GetPageResult(null, new Redirect(url, temporary), false);
    }

    public static GetPageResult ForError() {
        return new GetPageResult(null, null, true);
    }
}