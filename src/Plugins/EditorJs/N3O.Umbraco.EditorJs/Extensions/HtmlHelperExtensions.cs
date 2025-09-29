using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using N3O.Umbraco.EditorJs.Models;
using System.Threading.Tasks;

namespace N3O.Umbraco.EditorJs.Extensions;

public static class HtmlHelperExtensions {
    public static async Task<IHtmlContent> RenderEditorJs(this IHtmlHelper html, EditorJsModel model) {
        return await html.PartialAsync("~/Views/Partials/EditorJs/Blocks.cshtml", model);
    }
}