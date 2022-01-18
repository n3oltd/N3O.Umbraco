using N3O.Umbraco.Exceptions;
using N3O.Umbraco.Giving.Allocations.Lookups;
using N3O.Umbraco.Giving.Donations.Content;
using Umbraco.Cms.Core.Mapping;
using Umbraco.Extensions;

namespace N3O.Umbraco.Giving.Donations.Models {
    public class DonationOptionMapping : IMapDefinition {
        public void DefineMaps(IUmbracoMapper mapper) {
            mapper.Define<DonationOptionContent, DonationOptionRes>((_, _) => new DonationOptionRes(), Map);
        }

        // Umbraco.Code.MapAll -Id -Name
        private void Map(DonationOptionContent src, DonationOptionRes dest, MapperContext ctx) {
            dest.Type = src.Type;

            if (src.Type == AllocationTypes.Fund) {
                dest.Fund = src.Fund.IfNotNull(ctx.Map<FundDonationOptionContent, FundDonationOptionRes>);
            } else if (src.Type == AllocationTypes.Sponsorship) {
                dest.Sponsorship = src.Sponsorship.IfNotNull(ctx.Map<SponsorshipDonationOptionContent, SponsorshipDonationOptionRes>);
            } else {
                throw UnrecognisedValueException.For(src.Type);
            }
        }
    }
}