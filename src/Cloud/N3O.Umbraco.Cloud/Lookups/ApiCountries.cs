using N3O.Umbraco.Attributes;
using N3O.Umbraco.Cloud.Extensions;
using N3O.Umbraco.Cloud.Models;
using N3O.Umbraco.Lookups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace N3O.Umbraco.Cloud.Lookups;

[Order(int.MaxValue)]
public class ApiCountries : ApiLookupsCollection<Country> {
    private readonly ICdnClient _cdnClient;

    public ApiCountries(ICdnClient cdnClient) {
        _cdnClient = cdnClient;
    }
    
    protected override async Task<IReadOnlyList<Country>> FetchAsync(CancellationToken cancellationToken) {
        var publishedCountries = await _cdnClient.DownloadSubscriptionContentAsync<PublishedCountries>(SubscriptionFiles.Countries,
                                                                                                       JsonSerializers.Simple,
                                                                                                       cancellationToken);

        var countries = new List<Country>();

        foreach (var publishedCountry in publishedCountries.Countries.Select(x => x.Value)) {
            var country = new Country(publishedCountry.Id,
                                      publishedCountry.Name,
                                      null,
                                      publishedCountry.Iso2Code,
                                      publishedCountry.Iso3Code,
                                      publishedCountry.DialingCode,
                                      publishedCountry.LocalityOptional,
                                      publishedCountry.PostalCodeOptional);
            
            countries.Add(country);
        }

        return countries;
    }

    protected override TimeSpan CacheDuration => TimeSpan.FromHours(12);
}