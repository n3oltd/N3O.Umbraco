using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using N3O.Umbraco.Constants;
using N3O.Umbraco.Extensions;
using System.Threading.Tasks;
using Umbraco.Extensions;

namespace N3O.Umbraco.Bundling.TagHelpers;

[HtmlTargetElement($"{Prefixes.TagHelpers}css-bundle")]
public class CssBundleTagHelper : TagHelper {
    private readonly IBundler _bundler;

    public CssBundleTagHelper(IBundler bundler) {
        _bundler = bundler;
    }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output) {
        var cssFiles = await _bundler.GenerateCssUrlsAsync();

        if (cssFiles.None()) {
            output.SuppressOutput();
        } else {
            output.TagName = null;

            foreach (var cssFile in cssFiles) {
                var linkTag = new TagBuilder("link");
                linkTag.Attributes.Add("rel", "stylesheet");
                linkTag.Attributes.Add("href", cssFile);

                output.Content.AppendHtml(linkTag.ToHtmlString());
            }
        }
    }
}
