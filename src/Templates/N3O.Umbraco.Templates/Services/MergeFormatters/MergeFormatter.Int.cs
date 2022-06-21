using N3O.Umbraco.Localization;

namespace N3O.Umbraco.Templates;

public class IntMergeFormatter : MergeFormatter<int?> {
    public IntMergeFormatter(IFormatter formatter) : base(formatter) { }

    protected override string Format(int? value) {
        return Formatter.Number.Format(value);
    }
}
