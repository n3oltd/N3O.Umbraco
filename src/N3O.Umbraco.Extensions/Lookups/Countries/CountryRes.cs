namespace N3O.Umbraco.Lookups {
    public class CountryRes : NamedLookupRes {
        public string Iso2Code { get; set; }
        public string Iso3Code { get; set; }
        public bool LocalityOptional { get; set; }
        public bool PostalCodeOptional { get; set; }
    }
}
