using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using N3O.Umbraco.Analytics.Extensions;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Pages;
using System;
using Umbraco.Extensions;

namespace N3O.Umbraco.Analytics.TagHelpers {
    [HtmlTargetElement("n3o-ga-events")]
    public class GAEventsTagHelper : TagHelper {
        [HtmlAttributeName("model")]
        public IPageViewModel Model { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output) {
            if (Model == null) {
                throw new ArgumentException(nameof(Model));
            }

            if (!Model.GAEvents().HasValue(x => x.JavaScript)) {
                output.SuppressOutput();
            } else {
                output.TagName = null;

                var scriptTag = new TagBuilder("script");

                scriptTag.InnerHtml.AppendHtml("window.addEventListener('load', function () {");
                scriptTag.InnerHtml.AppendHtml("  if (window.gtag && typeof(window.gtag) === 'function') {");
                scriptTag.InnerHtml.AppendHtml(Model.GAEvents().JavaScript);
                scriptTag.InnerHtml.AppendHtml("  }");
                scriptTag.InnerHtml.AppendHtml("});");

                output.Content.SetHtmlContent(scriptTag.ToHtmlString());
            }
        }
    }
}