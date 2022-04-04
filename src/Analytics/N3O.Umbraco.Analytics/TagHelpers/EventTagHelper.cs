using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.TagHelpers;
using N3O.Umbraco.Json;
using System.Threading.Tasks;
using Umbraco.Extensions;

namespace N3O.Umbraco.Analytics.TagHelpers {
    public abstract class EventTagHelper : TagHelper {
        private readonly IJsonProvider _jsonProvider;

        protected EventTagHelper(IJsonProvider jsonProvider) {
            _jsonProvider = jsonProvider;
        }
        
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output) {
            if (IsSuppressed()) {
                output.SuppressOutput();
            } else {
                output.TagName = null;

                var parameters = await GetParametersAsync();
                var parametersJson = _jsonProvider.SerializeObject(parameters);
                
                var scriptTag = new TagBuilder("script");

                scriptTag.InnerHtml.AppendHtmlLine("window.addEventListener('load', function() {");
                scriptTag.InnerHtml.AppendHtmlLine("  if (typeof gtag === 'function') {");
                scriptTag.InnerHtml.AppendHtmlLine($"    gtag('event', '{EventName}', {parametersJson})");
                scriptTag.InnerHtml.AppendHtmlLine("  }");
                scriptTag.InnerHtml.AppendHtmlLine("}");

                output.Content.SetHtmlContent(scriptTag.ToHtmlString());
            }
        }

        protected virtual bool IsSuppressed() => false;
        protected abstract Task<object> GetParametersAsync();
        
        protected abstract string EventName { get; }
    }
}