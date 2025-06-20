﻿using N3O.Umbraco.Attributes;
using N3O.Umbraco.Cloud.Extensions;
using N3O.Umbraco.Cloud.Models;
using N3O.Umbraco.Lookups;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace N3O.Umbraco.Cloud.Lookups;

[Order(int.MaxValue)]
public class ApiCountries : ApiLookupsCollection<Country> {
    private readonly ICdnClient _cdnClient;

    public ApiCountries(ICdnClient cdnClient) {
        _cdnClient = cdnClient;
    }
    
    protected override async Task<IReadOnlyList<Country>> FetchAsync() {
        var publishedLookups = await _cdnClient.DownloadSubscriptionContentAsync<PublishedLookups>(SubscriptionFiles.Lookups);

        var countries = new List<Country>();

        foreach (var publishedCountry in publishedLookups.Countries) {
            var country = new Country(publishedCountry.Id,
                                      publishedCountry.Name,
                                      publishedCountry.Iso2Code,
                                      publishedCountry.Iso3Code,
                                      publishedCountry.DialingCode,
                                      publishedCountry.LocalityOptional,
                                      publishedCountry.PostalCodeOptional);
            
            countries.Add(country);
        }

        return countries;
    }
}