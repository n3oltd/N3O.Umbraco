using System.Threading;

namespace N3O.Umbraco.Localization;

public class LocalizationSettings : Value {
    public LocalizationSettings(string defaultCultureCode,
                                NumberFormat numberFormat,
                                DateFormat dateFormat,
                                TimeFormat timeFormat,
                                Timezone timezone) {
        DefaultCultureCode = defaultCultureCode;
        NumberFormat = numberFormat;
        DateFormat = dateFormat;
        TimeFormat = timeFormat;
        Timezone = timezone;
    }

    public string DefaultCultureCode { get; }
    public NumberFormat NumberFormat { get; }
    public DateFormat DateFormat { get; }
    public TimeFormat TimeFormat { get; }
    public Timezone Timezone { get; }
    
    public static string CultureCode => Thread.CurrentThread.CurrentCulture.ToString();
}
