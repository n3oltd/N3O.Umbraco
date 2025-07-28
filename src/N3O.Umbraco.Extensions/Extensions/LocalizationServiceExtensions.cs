using N3O.Umbraco.Localization;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Extensions;

public static class LocalizationServiceExtensions {
    public static IReadOnlyList<string> GetAllCultureCodes(this ILocalizationService localizationService) {
        try {
            return localizationService.GetAllLanguages().Select(x => x.IsoCode).OrderBy(x => x).ToList();
        } catch {
            return [ DefaultLocalizationSettingsAccessor.DefaultCultureCode ];
        }
    }
    
    public static string GetDefaultCultureCode(this ILocalizationService localizationService) {
        try {
            return localizationService.GetDefaultLanguageIsoCode();
        } catch {
            return DefaultLocalizationSettingsAccessor.DefaultCultureCode;
        }
    }
}