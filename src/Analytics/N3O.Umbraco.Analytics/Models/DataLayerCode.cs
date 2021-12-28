using Microsoft.AspNetCore.Html;

namespace N3O.Umbraco.Analytics.Models {
    public class DataLayerCode : Value {
        public DataLayerCode(HtmlString javaScript) {
            JavaScript = javaScript;
        }

        public HtmlString JavaScript { get; }
    }
}
