using Microsoft.AspNetCore.Html;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Json;
using System.Collections.Generic;
using System.Text;

namespace N3O.Umbraco.Analytics {
    public class DataLayerBuilder : IDataLayerBuilder {
        private readonly IJsonProvider _jsonProvider;

        public DataLayerBuilder(IJsonProvider jsonProvider) {
            _jsonProvider = jsonProvider;
        }
    
        public HtmlString BuildJavaScript(IEnumerable<object> toPush) {
            var javaScript = new StringBuilder();

            foreach (var obj in toPush.OrEmpty()) {
                var json = _jsonProvider.SerializeObject(obj);
                
                javaScript.AppendLine($"window.dataLayer.push({json});");
                javaScript.AppendLine();
            }

            return new HtmlString(javaScript.ToString());
        }
    }
}
