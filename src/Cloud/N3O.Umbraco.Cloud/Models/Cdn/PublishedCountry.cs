namespace N3O.Umbraco.Cloud.Models;

public class PublishedCountry : PublishedNamedLookup {
    public string Iso2Code { get; set; }
    public string Iso3Code { get; set; }
    public int DialingCode { get; set; }
    public bool LocalityOptional { get; set; }
    public bool PostalCodeOptional { get; set; }
}