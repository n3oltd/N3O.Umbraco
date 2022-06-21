using Microsoft.AspNetCore.Razor.TagHelpers;
using N3O.Umbraco.Extensions;
using System;
using Umbraco.Extensions;

namespace N3O.Umbraco.Markup.TagHelpers;

[HtmlTargetElement("n3o-markup")]
public class MarkupTagHelper : TagHelper {
    private readonly IMarkupEngine _markupEngine;

    public MarkupTagHelper(IMarkupEngine markupEngine) {
        _markupEngine = markupEngine;
    }
    
    [HtmlAttributeName("content")]
    public string Content { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output) {
        if (Content == null) {
            throw new ArgumentException(nameof(Content));
        }

        if (!Content.HasValue()) {
            output.SuppressOutput();
        } else {
            output.TagName = null;

            var html = _markupEngine.RenderHtml(Content);

            output.Content.SetHtmlContent(html.ToHtmlString());
        }
    }
}
