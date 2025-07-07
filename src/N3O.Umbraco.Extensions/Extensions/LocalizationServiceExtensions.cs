using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Extensions;

public static class LocalizationServiceExtensions {
    public static IReadOnlyList<string> GetAllCultureCodes(this ILocalizationService localizationService) {
        return localizationService.GetAllLanguages().Select(x => x.IsoCode).OrderBy(x => x).ToList();
    }
    
    public static string GetDefaultCultureCode(this ILocalizationService localizationService) {
        return localizationService.GetDefaultLanguageIsoCode();
    }
}