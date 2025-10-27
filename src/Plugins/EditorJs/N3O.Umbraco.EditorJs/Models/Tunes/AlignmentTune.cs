using Microsoft.AspNetCore.Html;
using N3O.Umbraco.EditorJs.Attributes;
using N3O.Umbraco.Extensions;

namespace N3O.Umbraco.EditorJs.Models;

[Tune("alignmentTune")]
public class AlignmentTune {
    public string Alignment { get; set; }

    public HtmlString ToStyle() {
        if (Alignment.HasValue()) {
            return new HtmlString($"style='text-align: {Alignment};'");
        } else {
            return null;
        }
    }
}