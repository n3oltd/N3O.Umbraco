using Microsoft.AspNetCore.Html;
using N3O.Umbraco.Analytics.Models;

namespace N3O.Umbraco.Analytics {
    public interface IGAEventsBuilder {
        HtmlString BuildJavaScript(GTag gTag);
    }
}
