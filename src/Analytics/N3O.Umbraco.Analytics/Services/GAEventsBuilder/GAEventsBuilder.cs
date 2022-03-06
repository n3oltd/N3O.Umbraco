using Microsoft.AspNetCore.Html;
using N3O.Umbraco.Analytics.Models;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Json;
using System.Text;

namespace N3O.Umbraco.Analytics {
    public class GAEventsBuilder : IGAEventsBuilder {
        private readonly IJsonProvider _jsonProvider;

        public GAEventsBuilder(IJsonProvider jsonProvider) {
            _jsonProvider = jsonProvider;
        }
    
        public HtmlString BuildJavaScript(GTag gTag) {
            var javaScript = new StringBuilder();

            foreach (var (name, parameters) in gTag.GetEvents()) {
                var parametersJson = parameters.IfNotNull(x => _jsonProvider.SerializeObject(x));

                if (parametersJson.HasValue()) {
                    javaScript.AppendLine($"window.gtag('event', '{name}', {parametersJson});");    
                } else {
                    javaScript.AppendLine($"window.gtag('event', '{name}');");
                }
                
                javaScript.AppendLine();
            }

            return new HtmlString(javaScript.ToString());
        }
    }
}
