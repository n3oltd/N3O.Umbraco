using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Cloud.Models;

public class PublishedCountry : PublishedNamedLookup, IHoldCountryCode {
    public string Iso2Code { get; set; }
    public string Iso3Code { get; set; }
    public int DialingCode { get; set; }
    public bool LocalityOptional { get; set; }
    public bool PostalCodeOptional { get; set; }
    
    string IHoldCountryCode.Iso2Or3Code => Iso3Code;
}