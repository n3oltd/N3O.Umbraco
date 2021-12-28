using N3O.Umbraco.Lookups;
using System.Collections.Generic;
using System.Linq;

namespace N3O.Umbraco.Extensions {
    public static class CountryExtensions {
        public static Country FindByCode(this IEnumerable<Country> countries, string iso2Or3Code) {
            var country = countries.SingleOrDefault(x => x.Iso2Code.EqualsInvariant(iso2Or3Code) ||
                                                         x.Iso3Code.EqualsInvariant(iso2Or3Code));

            return country;
        }
    }
}
