using Microsoft.AspNetCore.Html;

namespace N3O.Umbraco.Markup {
    public interface IMarkupEngine {
        HtmlString RenderHtml(string markup);
        bool Validate(string markup);
    }
}