namespace N3O.Umbraco.Localization;

public class LocalizationSettings : Value {
    public LocalizationSettings(NumberFormat numberFormat,
                                DateFormat dateFormat,
                                TimeFormat timeFormat,
                                Language language,
                                Timezone timezone) {
        NumberFormat = numberFormat;
        DateFormat = dateFormat;
        TimeFormat = timeFormat;
        Language = language;
        Timezone = timezone;
    }

    public NumberFormat NumberFormat { get; }
    public DateFormat DateFormat { get; }
    public TimeFormat TimeFormat { get; }
    public Language Language { get; }
    public Timezone Timezone { get; }
}
