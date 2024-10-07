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
            var numberFormat = Get("NumberFormat", NumberFormats.International);
            var dateFormat = Get("DateFormat", DateFormats.DayMonthYearSlashes);
            var timeFormat = Get("TimeFormat", TimeFormats._24);
            var language = Get("Language", Languages.English);
            var timezone = Get("Timezone", Timezones.Utc);
        
            _settings = new LocalizationSettings(numberFormat,
                                                 dateFormat,
                                                 timeFormat,
                                                 language,
                                                 timezone);
        }

        return _settings;
    }

    private T Get<T>(string setting, T defaultValue) where T : ILookup {
        var id = EnvironmentSettings.GetValue($"N3O_{setting}");
        var value = id.IfNotNull(x => _lookups.FindById<T>(id));

        return value ?? defaultValue;
    }
}
