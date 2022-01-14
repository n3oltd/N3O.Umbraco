using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Hosting;
using Umbraco.Extensions;

namespace N3O.Umbraco.TagHelpers {
    [HtmlTargetElement("n3o-report-issue")]
    public class ReportIssueTagHelper : TagHelper {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ReportIssueTagHelper(IWebHostEnvironment webHostEnvironment) {
            _webHostEnvironment = webHostEnvironment;
        }
        
        public override void Process(TagHelperContext context, TagHelperOutput output) {
            if (_webHostEnvironment.IsProduction()) {
                output.SuppressOutput();
            } else {
                output.TagName = null;

                var scriptTag = new TagBuilder("script");
                scriptTag.InnerHtml.AppendHtml("(function () {");
                scriptTag.InnerHtml.AppendHtml("  var s = document.createElement('script');");
                scriptTag.InnerHtml.AppendHtml("  s.type = 'text/javascript';");
                scriptTag.InnerHtml.AppendHtml("  s.async = true;");
                scriptTag.InnerHtml.AppendHtml("  s.src = '//api.usersnap.com/load/87997022-e6ff-4832-834d-c17f947fa639.js';");
                scriptTag.InnerHtml.AppendHtml("  var x = document.getElementsByTagName('script')[0];");
                scriptTag.InnerHtml.AppendHtml("  x.parentNode.insertBefore(s, x);");
                scriptTag.InnerHtml.AppendHtml("})();");

                output.Content.SetHtmlContent(scriptTag.ToHtmlString());
            }
        }
    }
}