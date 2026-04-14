using N3O.Umbraco.Cloud.Platforms.Clients;
using N3O.Umbraco.Extensions;

namespace N3O.Umbraco.Cloud.Platforms.Extensions;

public static class HtmlStringExtensions {
    public static RichTextContentReq ToRichTextContentReq(this string src) {
        if (!src.HasValue()) {
            return null;
        }
        
        var req = new RichTextContentReq();
        req.Html = src;

        return req;
    }
}
