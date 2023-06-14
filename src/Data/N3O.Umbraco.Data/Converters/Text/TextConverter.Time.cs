using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using NodaTime;
using NodaTime.Text;

namespace N3O.Umbraco.Data.Converters;

public class TimeTextConverter : ITextConverter<LocalTime?> {
    public string ToInvariantText(LocalTime? value) {
        return value.IfNotNull(x => LocalTimePattern.GeneralIso.Format(x));
    }

    public string ToText(IFormatter formatter, LocalTime? value) {
        return formatter.DateTime.FormatTime(value);
    }
}
