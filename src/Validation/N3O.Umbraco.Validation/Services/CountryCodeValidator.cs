using ISO3166;
using N3O.Umbraco.Extensions;
using System.Linq;

namespace N3O.Umbraco.Validation {
    public class CountryCodeValidator : ICountryCodeValidator {
        public Country Validate(string iso2Or3Code) {
            var country = Country.List
                                 .SingleOrDefault(x => x.ThreeLetterCode.EqualsInvariant(iso2Or3Code) ||
                                                       x.TwoLetterCode.EqualsInvariant(iso2Or3Code));

            return country;
        }
    }
}
