using Microsoft.AspNetCore.Razor.TagHelpers;
using N3O.Umbraco.Canonical;
using N3O.Umbraco.Constants;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Pages;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace N3O.Umbraco.TagHelpers;

[HtmlTargetElement($"{Prefixes.TagHelpers}canonical-url")]
public class CanonicalUrlTagHelper : TagHelper {
    private readonly IReadOnlyList<ICanonicalUrlProvider> _allProviders;

    public CanonicalUrlTagHelper(IEnumerable<ICanonicalUrlProvider> allProviders) {
        _allProviders = allProviders.ApplyAttributeOrdering();
    }

    [HtmlAttributeName("model")]
    public IPageViewModel Model { get; set; }

    public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output) {
        output.TagName = "link";
        output.TagMode = TagMode.SelfClosing;

        var url = await GetUrlAsync();

        output.Attributes.SetAttribute("rel", "canonical");
        output.Attributes.SetAttribute("href", url);
    }

    private async Task<string> GetUrlAsync() {
        var providers = _allProviders.Where(x => x.IsProviderFor(Model.Content));

        foreach (var provider in providers) {
            var url = await provider.GetUrlAsync(Model.Content);

            if (url.HasValue()) {
                return url;
            }
        }

        return Model.Content.AbsoluteUrl();
    }
}
