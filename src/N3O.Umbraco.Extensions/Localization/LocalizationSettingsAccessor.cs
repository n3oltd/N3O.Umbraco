using N3O.Umbraco.Constants;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Hosting;
using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Localization;

public class LocalizationSettingsAccessor : ILocalizationSettingsAccessor {
    private readonly ILookups _lookups;
    private LocalizationSettings _settings;

    public LocalizationSettingsAccessor(ILookups lookups) {
        _lookups = lookups;
    }

    public LocalizationSettings GetSettings() {
        if (_settings == null) {
            var numberFormat = Get(LocalizationKeys.NumberFormat, NumberFormats.International);
            var dateFormat = Get(LocalizationKeys.DateFormat, DateFormats.DayMonthYearSlashes);
            var timeFormat = Get(LocalizationKeys.TimeFormat, TimeFormats._24);
            var language = Get(LocalizationKeys.Language, Languages.English);
            var timezone = Get(LocalizationKeys.Timezone, Timezones.Utc);
        
            _settings = new LocalizationSettings(numberFormat, dateFormat, timeFormat, language, timezone);
        }

        return _settings;
    }

    private T Get<T>(string setting, T defaultValue) where T : ILookup {
        var id = EnvironmentData.GetOurValue(setting);
        var value = id.IfNotNull(x => _lookups.FindById<T>(x));

        return value ?? defaultValue;
    }
}
