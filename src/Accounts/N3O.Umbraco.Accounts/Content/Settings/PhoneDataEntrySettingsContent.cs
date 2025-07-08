using N3O.Umbraco.Accounts.Models;
using N3O.Umbraco.Content;
using N3O.Umbraco.Extensions;
using N3O.Umbraco.Lookups;
using N3O.Umbraco.Utilities;
using System;
using System.Linq;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace N3O.Umbraco.Accounts.Content;

public class PhoneDataEntrySettingsContent : UmbracoContent<PhoneDataEntrySettingsContent> {
    public bool Required => GetValue(x => x.Required);
    public string Label => GetValue(x => x.Label);
    public string HelpText => GetValue(x => x.HelpText);
    public bool Validate => GetValue(x => x.Validate);
    
    public Country GetDefaultCountry(ILookups lookups) {
        var propertyValue = Content().GetProperty(AccountsConstants.DataEntrySettings.Properties.DefaultCountry)
                                    ?.GetValue();
        Country country;
        
        if (propertyValue is IPublishedContent publishedContent) {
            var allCountries = lookups.GetAll<Country>();
            
            var countryCode = publishedContent.As<CountryContent>().Iso2Code;
            
            country = allCountries.SingleOrDefault(x => x.Iso2Code == countryCode);
        } else if (propertyValue is Country countryLookup) {
            country =  countryLookup;
        } else {
            throw new Exception("Default country must either be a picker or a datalist");
        }

        return country;
    }

    public PhoneDataEntrySettings ToDataEntrySettings(ILookups lookups) {
        SelectOption ToSelectOption(Country country) => new(country.Id, $"{country.Name} ({country.DialingCode})");
        
        var countryOptions = lookups.GetAll<Country>().Select(ToSelectOption).ToList();

        var defaultCountry = GetDefaultCountry(lookups);
        
        var countrySettings = new SelectFieldSettings(true,
                                                      Required,
                                                      Label,
                                                      HelpText,
                                                      HtmlField.Name<AccountReq>(x => x.Telephone.Country),
                                                      countryOptions,
                                                      1,
                                                      Validate,
                                                      defaultCountry.IfNotNull(ToSelectOption));
        
        var numberSettings = new PhoneFieldSettings(true,
                                                    Required,
                                                    Label,
                                                    HelpText,
                                                    HtmlField.Name<AccountReq>(x => x.Telephone.Number),
                                                    1,
                                                    Validate);
        
        return new PhoneDataEntrySettings(countrySettings, numberSettings);
    }
}
