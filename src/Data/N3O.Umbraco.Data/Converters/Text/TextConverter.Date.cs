using N3O.Umbraco.Localization;
using NodaTime;

namespace N3O.Umbraco.Data.Converters;

public class DateTextConverter : ITextConverter<LocalDate?> {
    public string Convert(IFormatter formatter, LocalDate? value) {
        return formatter.DateTime.FormatDate(value);
    }
}
