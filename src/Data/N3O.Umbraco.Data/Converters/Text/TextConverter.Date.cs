using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using NodaTime;
using NodaTime.Text;

namespace N3O.Umbraco.Data.Converters;

public class DateTextConverter : ITextConverter<LocalDate?> {
    public string ToInvariantText(LocalDate? value) {
        return value.IfNotNull(x => LocalDatePattern.Iso.Format(x));
    }

    public string ToText(IFormatter formatter, LocalDate? value) {
        return formatter.DateTime.FormatDate(value);
    }
}
