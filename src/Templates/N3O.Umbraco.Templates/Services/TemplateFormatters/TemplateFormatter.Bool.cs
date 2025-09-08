using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;

namespace N3O.Umbraco.Templates;

public class BoolTemplateFormatter : TemplateFormatter<bool?> {
    public BoolTemplateFormatter(IFormatter formatter) : base(formatter) { }

    protected override string Format(bool? value) {
        return value.ToYesNoString(Formatter.Text);
    }
}
