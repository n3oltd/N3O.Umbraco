namespace N3O.Umbraco.Localization;

public partial class DateTimeFormatter : IDateTimeFormatter {
    private readonly ILocalizationSettingsAccessor _settingsAccessor;

    public DateTimeFormatter(ILocalizationSettingsAccessor settingsAccessor) {
        _settingsAccessor = settingsAccessor;
    }

    public static readonly IDateTimeFormatter Invariant =
        new DateTimeFormatter(DefaultLocalizationSettingsAccessor.Instance);
}
