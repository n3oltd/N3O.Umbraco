using N3O.Umbraco.Constants;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Hosting;
using N3O.Umbraco.Lookups;
using System.Linq;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Localization;

public class EnvironmentLocalizationSettingsAccessor : ILocalizationSettingsAccessor {
    private readonly ILookups _lookups;
    private readonly ILanguageService _languageService;
    private LocalizationSettings _settings;

    public EnvironmentLocalizationSettingsAccessor(ILookups lookups, ILanguageService languageService) {
        _lookups = lookups;
        _languageService = languageService;
    }

    public LocalizationSettings GetSettings() {
        if (_settings == null) {
            var numberFormat = Get(LocalizationKeys.NumberFormat, NumberFormats.International);
            var dateFormat = Get(LocalizationKeys.DateFormat, DateFormats.DayMonthYearSlashes);
            var timeFormat = Get(LocalizationKeys.TimeFormat, TimeFormats._24);
            var timezone = Get(LocalizationKeys.Timezone, Timezones.Utc);

            var defaultLanguage = _languageService.GetDefaultLanguageAsync().GetAwaiter().GetResult();
            var allLanguages = _languageService.GetAllAsync().GetAwaiter().GetResult();

            _settings = new LocalizationSettings(defaultLanguage?.IsoCode,
                                                 allLanguages.Select(x => x.IsoCode).ToList(),
                                                 numberFormat,
                                                 dateFormat,
                                                 timeFormat,
                                                 timezone);
        }

        return _settings;
    }

    private T Get<T>(string setting, T defaultValue) where T : ILookup {
        var id = EnvironmentData.GetOurValue(setting);
        var value = id.IfNotNull(x => _lookups.FindById<T>(x));

        return value ?? defaultValue;
    }
}
