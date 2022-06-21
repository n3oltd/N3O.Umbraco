using N3O.Umbraco.Localization;
using System;

namespace N3O.Umbraco.Templates;

public class DateTimeMergeFormatter : MergeFormatter<DateTime?> {
    public DateTimeMergeFormatter(IFormatter formatter) : base(formatter) { }

    protected override string Format(DateTime? value) {
        return Formatter.DateTime.FormatDate(value);
    }
}
