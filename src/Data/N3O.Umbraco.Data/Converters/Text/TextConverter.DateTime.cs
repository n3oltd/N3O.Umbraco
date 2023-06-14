using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using NodaTime;
using NodaTime.Text;

namespace N3O.Umbraco.Data.Converters;

public class DateTimeTextConverter : ITextConverter<LocalDateTime?> {
    public string ToInvariantText(LocalDateTime? value) {
        return value.IfNotNull(x => LocalDateTimePattern.GeneralIso.Format(x));
    }

    public string ToText(IFormatter formatter, LocalDateTime? value) {
        return formatter.DateTime.FormatDateWithTime(value);
    }
}
