namespace N3O.Umbraco.Lookups;

public class Country : NamedLookup, IHoldCountryCode {
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
    
    string IHoldCountryCode.Iso2Or3Code => Iso3Code;
}