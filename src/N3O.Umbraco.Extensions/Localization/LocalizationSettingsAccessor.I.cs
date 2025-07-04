using System.Collections.Generic;

namespace N3O.Umbraco.Localization;

public interface ILocalizationSettingsAccessor {
    LocalizationSettings GetSettings();
    IEnumerable<string> GetAllAvailableCultures();
}
