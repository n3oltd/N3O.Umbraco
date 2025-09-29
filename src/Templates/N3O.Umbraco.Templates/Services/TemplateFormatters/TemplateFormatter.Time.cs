using N3O.Umbraco.Localization;
using NodaTime;

namespace N3O.Umbraco.Templates;

public class TimeTemplateFormatter : TemplateFormatter<LocalTime?> {
    public TimeTemplateFormatter(IFormatter formatter) : base(formatter) { }

    protected override string Format(LocalTime? value) {
        return Formatter.DateTime.FormatTime(value);
    }
}
