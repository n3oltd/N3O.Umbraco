using NodaTime;

namespace N3O.Umbraco.Search.Extensions;

public static class LocalDateExtensions {
    public static bool HasValue(this LocalDate? date) {
        if (date == null || date.Value == default) {
            return false;
        }

        return true;
    }
}
