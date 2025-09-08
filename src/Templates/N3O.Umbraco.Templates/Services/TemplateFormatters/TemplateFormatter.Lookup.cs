using N3O.Umbraco.Localization;
using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Templates;

public class LookupTemplateFormatter : TemplateFormatter<INamedLookup> {
    public LookupTemplateFormatter(IFormatter formatter) : base(formatter) { }

    protected override string Format(INamedLookup value) {
        return Formatter.Text.FormatLookupName(value);
    }
}
