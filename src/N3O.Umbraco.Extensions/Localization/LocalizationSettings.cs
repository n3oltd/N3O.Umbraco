using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace N3O.Umbraco.Localization;

public class LocalizationSettings : Value {
    public LocalizationSettings(string defaultCultureCode,
                                IEnumerable<string> allCultureCodes,
                                NumberFormat numberFormat,
                                DateFormat dateFormat,
                                TimeFormat timeFormat,
                                Timezone timezone) {
        DefaultCultureCode = defaultCultureCode;
        AllCultureCodes = allCultureCodes.ToList();
        NumberFormat = numberFormat;
        DateFormat = dateFormat;
        TimeFormat = timeFormat;
        Timezone = timezone;
    }

    public string DefaultCultureCode { get; }
    public IReadOnlyList<string> AllCultureCodes { get; }
    public NumberFormat NumberFormat { get; }
    public DateFormat DateFormat { get; }
    public TimeFormat TimeFormat { get; }
    public Timezone Timezone { get; }

    public static string GetLanguageName(string cultureCode) {
        try {
            var requestedCulture = CultureInfo.GetCultureInfo(cultureCode);
            var languageCulture = CultureInfo.GetCultureInfo(requestedCulture.TwoLetterISOLanguageName);

            return languageCulture.EnglishName;
        } catch {
            return null;
        }
    }
}
