using N3O.Umbraco.Localization;

namespace N3O.Umbraco.Templates;

public class DecimalTemplateFormatter : TemplateFormatter<decimal?> {
    public DecimalTemplateFormatter(IFormatter formatter) : base(formatter) { }

    protected override string Format(decimal? value) {
        return Formatter.Number.Format(value);
    }
}
