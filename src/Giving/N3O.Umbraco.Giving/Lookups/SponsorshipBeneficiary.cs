using N3O.Umbraco.Extensions;
using N3O.Umbraco.Lookups;

namespace N3O.Umbraco.Giving.Lookups {
    public class SponsorshipBeneficiary : LookupContent<SponsorshipBeneficiary> {
        public SponsorshipScheme Scheme => Content().Parent.As<SponsorshipScheme>();
    }
}