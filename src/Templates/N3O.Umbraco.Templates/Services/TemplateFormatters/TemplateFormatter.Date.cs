using N3O.Umbraco.Localization;
using NodaTime;

namespace N3O.Umbraco.Templates;

public class DateTemplateFormatter : TemplateFormatter<LocalDate?> {
    public DateTemplateFormatter(IFormatter formatter) : base(formatter) { }

    protected override string Format(LocalDate? value) {
        return Formatter.DateTime.FormatDate(value);
    }
}
