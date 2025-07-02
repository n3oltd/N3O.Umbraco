using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using N3O.Umbraco.Cloud.Lookups;
using Umbraco.Extensions;

namespace N3O.Umbraco.Cloud.Platforms.TagHelpers;

[HtmlTargetElement("n3o-platforms-js")]
public class PlatformsJsTagHelper : TagHelper {
    private readonly ICloudUrl _cloudUrl;

    public PlatformsJsTagHelper(ICloudUrl cloudUrl) {
        _cloudUrl = cloudUrl;
    }

    public override void Process(TagHelperContext context, TagHelperOutput output) {
        output.TagName = null;

        var scriptTag = new TagBuilder("script");
        scriptTag.Attributes.Add("type", "module");
        scriptTag.Attributes.Add("async", "async");
        scriptTag.Attributes.Add("defer", "defer");
        scriptTag.Attributes.Add("src", _cloudUrl.ForCdn(CdnRoots.Connect, "platforms/js/platforms.js"));

        output.Content.AppendHtmlLine("<!-- N3O Platforms JS (https://n3o.ltd) -->");
        output.Content.AppendHtml(scriptTag.ToHtmlString());
    }
}
