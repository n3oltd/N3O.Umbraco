namespace N3O.Umbraco.Localization;

public partial class DateTimeFormatter : IDateTimeFormatter {
    private readonly ILocalizationSettingsAccessor _settingsAccessor;
    private DateFormat _dateFormat;
    private TimeFormat _timeFormat;
    private Timezone _timezone;

    public DateTimeFormatter(ILocalizationSettingsAccessor settingsAccessor) {
        _settingsAccessor = settingsAccessor;
    }

    private DateTimeFormatter(LocalizationSettings settings) {
        _dateFormat = settings.DateFormat;
        _timeFormat = settings.TimeFormat;
        _timezone = settings.Timezone;
    }

    public static readonly IDateTimeFormatter Default =
        new DateTimeFormatter(DefaultLocalizationSettingsAccessor.Instance);

    public DateFormat DateFormat => _dateFormat ??= _settingsAccessor.GetSettings().DateFormat;
    public TimeFormat TimeFormat => _timeFormat ??= _settingsAccessor.GetSettings().TimeFormat;
    public Timezone Timezone => _timezone ??= _settingsAccessor.GetSettings().Timezone;

    public static IDateTimeFormatter Create(LocalizationSettings settings) {
        return new DateTimeFormatter(settings);
    }
}
