using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Localization;

namespace N3O.Umbraco.Data.Converters;

public class CalendarMonthTextConverter : ITextConverter<CalendarMonth> {
    public string ToInvariantText(CalendarMonth value) {
        return value?.ToString();
    }

    public string ToText(IFormatter formatter, CalendarMonth value) {
        return value?.ToString();
    }
}
