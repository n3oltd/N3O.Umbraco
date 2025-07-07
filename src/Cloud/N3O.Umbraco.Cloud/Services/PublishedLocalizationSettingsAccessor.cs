using N3O.Umbraco.Cloud.Extensions;
using N3O.Umbraco.Cloud.Lookups;
using N3O.Umbraco.Cloud.Models;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Localization;
using N3O.Umbraco.Lookups;
using System.Collections.Generic;
using Umbraco.Cms.Core.Services;

namespace N3O.Umbraco.Cloud;

public class PublishedLocalizationSettingsAccessor : ILocalizationSettingsAccessor {
    private readonly ICdnClient _cdnClient;
    private readonly ILookups _lookups;
    private readonly ILocalizationService _localizationService;
    private LocalizationSettings _localizationSettings;

    public PublishedLocalizationSettingsAccessor(ICdnClient cdnClient,
                                                 ILookups lookups,
                                                 ILocalizationService localizationService) {
        _cdnClient = cdnClient;
        _lookups = lookups;
        _localizationService = localizationService;
    }

    public IEnumerable<string> GetAllAvailableCultures() => _localizationService.GetAllCultureCodes();

    public LocalizationSettings GetSettings() {
        if (_localizationSettings == null) {
            var publishedLocalization = _cdnClient.DownloadSubscriptionContentAsync<PublishedLocalization>(SubscriptionFiles.Localization,
                                                                                                           JsonSerializers.JsonProvider)
                                                  .GetAwaiter()
                                                  .GetResult();

            var timezone = _lookups.FindById<Timezone>(publishedLocalization.Timezone.Id);

            _localizationSettings = new LocalizationSettings(_localizationService.GetDefaultCultureCode(),
                                                             _localizationService.GetAllCultureCodes(),
                                                             publishedLocalization.NumberFormat,
                                                             publishedLocalization.DateFormat,
                                                             publishedLocalization.TimeFormat,
                                                             timezone);
        }

        return _localizationSettings;
    }
}