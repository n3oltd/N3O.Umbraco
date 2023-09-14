using N3O.Umbraco.Data.Models;
using N3O.Umbraco.Localization;

namespace N3O.Umbraco.Data.Converters;

public class CalendarWeekTextConverter : ITextConverter<CalendarWeek> {
    public string ToInvariantText(CalendarWeek value) {
        return value?.ToString();
    }

    public string ToText(IFormatter formatter, CalendarWeek value) {
        return value?.ToString();
    }
}