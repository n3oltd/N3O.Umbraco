using N3O.Umbraco.Localization;
using NodaTime;

namespace N3O.Umbraco.Templates;

public class TimeMergeFormatter : MergeFormatter<LocalTime?> {
    public TimeMergeFormatter(IFormatter formatter) : base(formatter) { }

    protected override string Format(LocalTime? value) {
        return Formatter.DateTime.FormatTime(value);
    }
}
