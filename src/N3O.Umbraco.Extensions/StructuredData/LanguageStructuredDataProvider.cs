using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using System.Linq;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.StructuredData;

public class LanguageStructuredDataProvider : IStructuredDataProvider {
    private readonly IVariationContextAccessor _variationContextAccessor;
    private readonly ILocalizationSettingsAccessor _localizationSettingsAccessor;

    public LanguageStructuredDataProvider(IVariationContextAccessor variationContextAccessor,
                                          ILocalizationSettingsAccessor localizationSettingsAccessor) {
        _variationContextAccessor = variationContextAccessor;
        _localizationSettingsAccessor = localizationSettingsAccessor;
    }

    public bool IsProviderFor(IPublishedContent page) {
        return true;
    }

    public void AddStructuredData(JsonLd jsonLd, IPublishedContent page) {
        var publishedCultureCodes = page.OrEmpty(x => x.Cultures)
                                        .Select(x => x.Key)
                                        .Where(x => x.HasValue())
                                        .ToList();

        if (publishedCultureCodes.Count < 2) {
            return;
        }

        var currentCultureCode = _variationContextAccessor?.VariationContext?.Culture;

        if (!currentCultureCode.HasValue()) {
            return;
        }

        var currentUrl = page.AbsoluteUrl(currentCultureCode);

        if (!IsRoutable(currentUrl)) {
            return;
        }

        jsonLd.OfType("WebPage")
              .Id(GetWebPageId(currentUrl))
              .Url(currentUrl)
              .InLanguage(GetLanguage(currentCultureCode));

        var defaultCultureCode = _localizationSettingsAccessor.GetSettings().DefaultCultureCode;

        foreach (var otherCultureCode in publishedCultureCodes.Where(x => !x.EqualsInvariant(currentCultureCode))) {
            var otherUrl = page.AbsoluteUrl(otherCultureCode);

            if (!IsRoutable(otherUrl)) {
                continue;
            }

            if (currentCultureCode.EqualsInvariant(defaultCultureCode)) {
                jsonLd.WorkTranslation(GetWebPageId(otherUrl));
            } else if (otherCultureCode.EqualsInvariant(defaultCultureCode)) {
                jsonLd.TranslationOfWork(GetWebPageId(otherUrl));
            }
        }
    }

    private static string GetWebPageId(string url) {
        return $"{url}#webpage";
    }

    private static string GetLanguage(string cultureCode) {
        var dashIndex = cultureCode.IndexOf('-');

        return (dashIndex > 0 ? cultureCode.Substring(0, dashIndex) : cultureCode).ToLowerInvariant();
    }

    private static bool IsRoutable(string url) {
        return url.HasValue() && !url.EndsWith("#");
    }
}
