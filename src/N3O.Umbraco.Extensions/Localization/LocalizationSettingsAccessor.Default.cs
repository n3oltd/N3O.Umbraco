namespace N3O.Umbraco.Localization;
public class DefaultLocalizationSettingsAccessor : ILocalizationSettingsAccessor {
    private readonly LocalizationSettings _settings;

    private DefaultLocalizationSettingsAccessor() {
        _settings = new LocalizationSettings(NumberFormats.International,
                                             DateFormats.DayMonthYearSlashes,
                                             TimeFormats._24,
                                             Languages.English,
                                             Timezones.Utc);
    }

    public LocalizationSettings GetSettings() {
        return _settings;
    }

    public static readonly ILocalizationSettingsAccessor Instance = new DefaultLocalizationSettingsAccessor();
}