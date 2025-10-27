using N3O.Umbraco.EditorJs.Attributes;
using N3O.Umbraco.Extensions;

namespace N3O.Umbraco.EditorJs.Model.Tunes;

[Tune("alignmentTune")]
public class AlignmentTune {
    public string Alignment { get; set; }

    public string ToStyle() {
        if (Alignment.HasValue()) {
            return $"""style='text-align: {Alignment};'""";
        } else {
            return null;
        }
    }
}