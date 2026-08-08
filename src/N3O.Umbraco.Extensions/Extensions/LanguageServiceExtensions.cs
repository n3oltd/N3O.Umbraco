using N3O.Umbraco.Localization;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Extensions;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Extensions;

public static class LanguageServiceExtensions {
    public static IReadOnlyList<string> GetAllCultureCodes(this ILanguageService languageService) {
        try {
            return languageService.GetAllIsoCodesAsync().GetAwaiter().GetResult().OrderBy(x => x).ToList();
        } catch {
            return [ DefaultLocalizationSettingsAccessor.DefaultCultureCode ];
        }
    }

    public static string GetDefaultCultureCode(this ILanguageService languageService) {
        try {
            return languageService.GetDefaultIsoCodeAsync().GetAwaiter().GetResult();
        } catch {
            return DefaultLocalizationSettingsAccessor.DefaultCultureCode;
        }
    }
}
