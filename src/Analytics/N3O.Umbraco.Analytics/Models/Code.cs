using Microsoft.AspNetCore.Html;
using N3O.Umbraco.Extensions;

namespace N3O.Umbraco.Analytics.Models;

public class Code : Value {
    public Code(HtmlString javaScript) {
        JavaScript = javaScript;
    }

    public HtmlString JavaScript { get; }

    public bool HasValue() {
        return JavaScript.HasValue();
    }
}
