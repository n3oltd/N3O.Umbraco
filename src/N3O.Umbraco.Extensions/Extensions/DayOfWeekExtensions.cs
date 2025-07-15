using N3O.Umbraco.Constants;
using N3O.Umbraco.Localization;
using System;

namespace N3O.Umbraco.Extensions;

public static class DayOfWeekExtensions {
    public static string ToString(this DayOfWeek dayOfWeek, IStringLocalizer stringLocalizer) {
        return stringLocalizer.Get(TextFolders.Lookups, "DayOfWeek", dayOfWeek.ToString());
    }
}