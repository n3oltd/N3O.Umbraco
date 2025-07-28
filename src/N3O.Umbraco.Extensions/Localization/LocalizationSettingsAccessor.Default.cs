using System.Collections.Generic;

namespace N3O.Umbraco.Localization;

public class DefaultLocalizationSettingsAccessor : ILocalizationSettingsAccessor {
    public const string DefaultCultureCode = "en-US";
    private readonly LocalizationSettings _settings;

    private DefaultLocalizationSettingsAccessor() {
        _settings = new LocalizationSettings(DefaultCultureCode,
                                             [DefaultCultureCode],
                                             NumberFormats.International,
                                             DateFormats.DayMonthYearSlashes,
                                             TimeFormats._24,
                                             Timezones.Utc);
    }
    
    public IEnumerable<string> GetAllAvailableCultures() {
        return [DefaultCultureCode];
    }

    public LocalizationSettings GetSettings() {
        return _settings;
    }

    public static readonly ILocalizationSettingsAccessor Instance = new DefaultLocalizationSettingsAccessor();
}
