using Microsoft.AspNetCore.Html;

namespace N3O.Umbraco.Analytics.Models {
    public class GoogleTagManagerCode : Value {
        public GoogleTagManagerCode(HtmlString body, HtmlString head) {
            Body = body;
            Head = head;
        }

        public HtmlString Body { get; }
        public HtmlString Head { get; }
    }
}
