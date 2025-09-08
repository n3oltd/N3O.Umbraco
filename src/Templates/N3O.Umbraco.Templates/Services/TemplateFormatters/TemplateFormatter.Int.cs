using N3O.Umbraco.Localization;

namespace N3O.Umbraco.Templates;

public class IntTemplateFormatter : TemplateFormatter<int?> {
    public IntTemplateFormatter(IFormatter formatter) : base(formatter) { }

    protected override string Format(int? value) {
        return Formatter.Number.Format(value);
    }
}
