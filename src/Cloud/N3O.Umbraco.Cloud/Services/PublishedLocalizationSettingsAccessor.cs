using N3O.Umbraco.Cloud.Extensions;
using N3O.Umbraco.Cloud.Lookups;
using N3O.Umbraco.Cloud.Models;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Lookups;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Cloud;

public class PublishedLocalizationSettingsAccessor : ILocalizationSettingsAccessor {
    private readonly ICdnClient _cdnClient;
    private readonly ILookups _lookups;
    private readonly ILanguageService _languageService;
    private LocalizationSettings _localizationSettings;

    public PublishedLocalizationSettingsAccessor(ICdnClient cdnClient,
                                                 ILookups lookups,
                                                 ILanguageService languageService) {
        _cdnClient = cdnClient;
        _lookups = lookups;
        _languageService = languageService;
    }

    public IEnumerable<string> GetAllAvailableCultures() {
        return _languageService.GetAllAsync().GetAwaiter().GetResult().Select(x => x.IsoCode);
    }

    public LocalizationSettings GetSettings() {
        if (_localizationSettings == null) {
            var publishedLocalization = _cdnClient.DownloadSubscriptionContentAsync<PublishedLocalization>(SubscriptionFiles.Localization,
                                                                                                           JsonSerializers.JsonProvider)
                                                  .GetAwaiter()
                                                  .GetResult();

            if (publishedLocalization == null) {
                return null;
            }

            var timezone = _lookups.FindById<Timezone>(publishedLocalization.Timezone.Id);
            var defaultLanguage = _languageService.GetDefaultLanguageAsync().GetAwaiter().GetResult();
            var allLanguages = _languageService.GetAllAsync().GetAwaiter().GetResult();

            _localizationSettings = new LocalizationSettings(defaultLanguage?.IsoCode,
                                                             allLanguages.Select(x => x.IsoCode).ToList(),
                                                             publishedLocalization.NumberFormat,
                                                             publishedLocalization.DateFormat,
                                                             publishedLocalization.TimeFormat,
                                                             timezone);
        }

        return _localizationSettings;
    }
}
