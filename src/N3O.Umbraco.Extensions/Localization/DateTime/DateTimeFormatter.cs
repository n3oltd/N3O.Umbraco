namespace N3O.Umbraco.Localization;

public partial class DateTimeFormatter : IDateTimeFormatter {
    public DateTimeFormatter(ILocalizationSettingsAccessor settingsAccessor) : this(settingsAccessor.GetSettings()) { }

    private DateTimeFormatter(LocalizationSettings settings) {
        DateFormat = settings.DateFormat;
        TimeFormat = settings.TimeFormat;
        Timezone = settings.Timezone;
    }

    public static readonly IDateTimeFormatter Invariant =
        new DateTimeFormatter(DefaultLocalizationSettingsAccessor.Instance);

    public DateFormat DateFormat { get; }
    public TimeFormat TimeFormat { get; }
    public Timezone Timezone { get; }

    public static IDateTimeFormatter Create(LocalizationSettings settings) {
        return new DateTimeFormatter(settings);
    }
}
