namespace N3O.Umbraco.Lookups;

public class Country : LookupContent {
    public string Iso2Code => GetValue<Country, string>(x => x.Iso2Code);
    public string Iso3Code => GetValue<Country, string>(x => x.Iso3Code);
    public bool LocalityOptional => GetValue<Country, bool>(x => x.LocalityOptional);
    public bool PostalCodeOptional => GetValue<Country, bool>(x => x.PostalCodeOptional);
}
