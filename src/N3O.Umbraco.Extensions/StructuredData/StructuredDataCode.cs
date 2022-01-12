using Microsoft.AspNetCore.Html;
using N3O.Umbraco.Extensions;

namespace N3O.Umbraco.Analytics.Models {
    public class StructuredDataCode : Value {
        public StructuredDataCode(HtmlString javaScriptObject) {
            JavaScriptObject = javaScriptObject;
        }

        public HtmlString JavaScriptObject { get; }

        public bool HasValue() {
            return JavaScriptObject.HasValue();
        }
    }
}
