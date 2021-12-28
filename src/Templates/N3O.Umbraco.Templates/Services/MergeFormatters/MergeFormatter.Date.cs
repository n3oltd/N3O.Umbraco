using N3O.Umbraco.Localization;
using NodaTime;

namespace N3O.Umbraco.Templates;

public class DateMergeFormatter : MergeFormatter<LocalDate?> {
    public DateMergeFormatter(IFormatter formatter) : base(formatter) { }
    
    protected override string Format(LocalDate? value) {
        return Formatter.DateTime.FormatDate(value);
    }
}
