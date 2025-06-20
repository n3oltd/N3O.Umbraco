using N3O.Umbraco.Attributes;
using N3O.Umbraco.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Umbraco.Cms.Core.Web;

namespace N3O.Umbraco.Lookups;

public class Country : NamedLookup {
    public Country(string id,
                   string name,
                   string iso2Code,
                   string iso3Code,
                   int dialingCode,
                   bool localityOptional,
                   bool postalCodeOptional)
        : base(id, name) {
        Iso2Code = iso2Code;
        Iso3Code = iso3Code;
        DialingCode = dialingCode;
        LocalityOptional = localityOptional;
        PostalCodeOptional = postalCodeOptional;
    }

    public string Iso2Code { get; }
    public string Iso3Code { get; }
    public int DialingCode { get; }
    public bool LocalityOptional { get; }
    public bool PostalCodeOptional { get; }
}

public class CountryContent : UmbracoContent<CountryContent> {
    public string Iso2Code => GetValue(x => x.Iso2Code);
    public string Iso3Code => GetValue(x => x.Iso3Code);
    [UmbracoProperty("diallingCode")]
    public int DialingCode => GetConvertedValue<string, int>(x => x.DialingCode, int.Parse);
    public bool LocalityOptional => GetValue(x => x.LocalityOptional);
    public bool PostalCodeOptional => GetValue(x => x.PostalCodeOptional);
}

[Order(int.MinValue)]
public class CountriesCollection : LookupsCollection<Country> {
    private readonly IContentCache _contentCache;
    private readonly IUmbracoContextAccessor _umbracoContextAccessor;

    public CountriesCollection(IContentCache contentCache, IUmbracoContextAccessor umbracoContextAccessor) {
        _contentCache = contentCache;
        _umbracoContextAccessor = umbracoContextAccessor;

        _contentCache.Flushed += ContentCacheOnFlushed;
    }
    
    protected override Task<IReadOnlyList<Country>> LoadAllAsync() {
        var all = GetFromCache();
        
        return Task.FromResult(all);
    }

    private IReadOnlyList<Country> GetFromCache() {
        List<CountryContent> content;
        
        if (_umbracoContextAccessor.TryGetUmbracoContext(out _)) {
            content = _contentCache.All<CountryContent>().OrderBy(x => x.Content().SortOrder).ToList();
        } else {
            content = new();
        }
        
        var lookups = content.Select(ToCountry).ToList();

        return lookups;
    }

    private Country ToCountry(CountryContent countryContent) {
        return new Country(LookupContent.GetId(countryContent.Content()),
                           LookupContent.GetName(countryContent.Content()),
                           countryContent.Iso2Code,
                           countryContent.Iso3Code,
                           countryContent.DialingCode,
                           countryContent.LocalityOptional,
                           countryContent.PostalCodeOptional);
    }

    private void ContentCacheOnFlushed(object sender, EventArgs e) {
        var all = GetFromCache();
        
        Reload(all);
    }
}