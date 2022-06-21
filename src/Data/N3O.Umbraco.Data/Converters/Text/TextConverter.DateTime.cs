using N3O.Umbraco.Localization;
using NodaTime;

namespace N3O.Umbraco.Data.Converters;

public class DateTimeTextConverter : ITextConverter<LocalDateTime?> {
    public string Convert(IFormatter formatter, LocalDateTime? value) {
        return formatter.DateTime.FormatDateWithTime(value);
    }
}
