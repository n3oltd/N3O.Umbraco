using ISO3166;

namespace N3O.Umbraco.Validation {
    public interface ICountryCodeValidator {
        Country Validate(string iso2Or3Code);
    }
}
