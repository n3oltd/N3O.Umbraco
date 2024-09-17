using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using N3O.Umbraco.Extensions;
using System.Threading.Tasks;
using Umbraco.Extensions;

namespace N3O.Umbraco.Bundling.TagHelpers;

[HtmlTargetElement("n3o-js-bundle")]
public class JsBundleTagHelper : TagHelper {
    private readonly IBundler _bundler;

    public JsBundleTagHelper(IBundler bundler) {
        _bundler = bundler;
    }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output) {
        var jsFiles = await _bundler.GenerateJsUrlsAsync();

        if (jsFiles.None()) {
            output.SuppressOutput();
        } else {
            output.TagName = null;

            foreach (var jsFile in jsFiles) {
                var scriptTag = new TagBuilder("script");
                scriptTag.Attributes.Add("src", jsFile);

                output.Content.AppendHtml(scriptTag.ToHtmlString());
            }
        }
    }
}
