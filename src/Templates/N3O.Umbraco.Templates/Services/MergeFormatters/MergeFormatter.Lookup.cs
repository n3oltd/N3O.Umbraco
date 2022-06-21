using N3O.Umbraco.Localization;
using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Templates;

public class LookupFormatter : MergeFormatter<INamedLookup> {
    public LookupFormatter(IFormatter formatter) : base(formatter) { }

    protected override string Format(INamedLookup value) {
        return Formatter.Text.FormatLookupName(value);
    }
}
